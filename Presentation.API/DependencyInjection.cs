﻿using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Genres;
using Domain.Entities.Platforms;
using Domain.Entities.Users;
using Domain.Enums;
using Domain.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Presentation.API.Services;
using Presentration.API.BackgroundJobs;
using Presentration.API.Options;
using Presentration.API.Services;
using System.Text;

namespace Presentration.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<UserId>());
                options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<CompanyId>());
                options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<GenreId>());
                options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<PlatformId>());
                options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<GenreId>());
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(setupAction =>
        {
            setupAction.AddSecurityDefinition("DDDBearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Input a valid token to access this API"
            });

            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "DDDBearerAuth"
                        }
                    },
                    new List<string>()
                }
            });
        });

        services.AddSwaggerGenNewtonsoftSupport();

        services.AddOutputCache(options =>
        {
            options.AddPolicy("Companies", builder =>
                builder.Expire(TimeSpan.FromMinutes(5))
                .Tag("companies"));

            options.AddPolicy("Games", builder =>
                builder.Expire(TimeSpan.FromMinutes(5))
                .Tag("games"));

            options.AddPolicy("Users", builder =>
                builder.Expire(TimeSpan.FromMinutes(5))
                .Tag("users"));

            options.AddPolicy("Genres", builder =>
                builder.Expire(TimeSpan.FromMinutes(5))
                .Tag("genres"));

            options.AddPolicy("Platforms", builder =>
                builder.Expire(TimeSpan.FromMinutes(5))
                .Tag("platforms"));
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:SecretForKey"]!))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/commentsHub"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.Configure<AuthOptions>(configuration.GetSection("Authentication"));
        services.Configure<SuperAdminOptions>(configuration.GetSection("SuperAdmin"));
        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.Configure<RecommendedGamesJobOptions>(configuration.GetSection("RecommendedGames"));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthenticationService>();
        services.AddScoped<IScheduler, Scheduler>();
        services.AddScoped<IVideoService, VideoService>();

        services.AddHttpContextAccessor();

        //services.AddHostedService<RecomendedGamesJob>();

        services.AddHangfire((provider, config) =>
        {
            Console.WriteLine(configuration.GetConnectionString("HangfireConnection"));
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));
            config.UseFilter(new AutomaticRetryAttribute { Attempts = 5 });
        });

        services.AddHangfireServer();

        services.AddSignalR(config =>
        {
            config.EnableDetailedErrors = true;
        });

        return services;
    }

    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
            tracerProviderBuilder
                .AddSource(GameEvaluatorMetricsService.SourceName)
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(GameEvaluatorMetricsService.SourceName).AddTelemetrySdk())
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter = (req) => !req.Request.Path.ToUriComponent().Contains("index.html", StringComparison.OrdinalIgnoreCase)
                        && !req.Request.Path.ToUriComponent().Contains("swagger", StringComparison.OrdinalIgnoreCase);
                })
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("OpenTelemetry:OtelEndpoint")!);
                })
                .AddSqlClientInstrumentation(options =>
                {
                    options.SetDbStatementForText = true;
                    options.RecordException = true;
                })
        ).WithMetrics(metricsProviderBuilder =>
            metricsProviderBuilder
               .AddHttpClientInstrumentation()
               .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(GameEvaluatorMetricsService.SourceName).AddTelemetrySdk())
               .ConfigureResource(resource => resource.AddService(GameEvaluatorMetricsService.SourceName))
               .AddMeter(GameEvaluatorMetricsService.Meter.Name)
               .AddOtlpExporter(otlpOptions =>
               {
                   otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("OpenTelemetry:OtelEndpoint")!);
               })
        );

        services.Configure<AspNetCoreInstrumentationOptions>(options =>
        {
            options.RecordException = true;
        });

        return services;
    }


    public static async Task AddSuperAdminRole(this IServiceScope scope)
    {
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var admin = scope.ServiceProvider.GetRequiredService<IOptions<SuperAdminOptions>>().Value;

        var foundAdminUser = await (await userRepository.GetAsync())
            .FirstOrDefaultAsync(user => user.Email.Equals(admin.Email));

        if (foundAdminUser is null)
        {
            var adminUser = User.Create(admin.Email, admin.Email, admin.Password);
            adminUser.AddRole(RoleType.SuperAdmin);
            adminUser.RemoveRole(RoleType.User);

            await userRepository.AddAsync(adminUser);
        }
    }
}