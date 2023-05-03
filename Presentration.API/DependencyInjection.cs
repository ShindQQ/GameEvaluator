using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Genres;
using Domain.Entities.Platforms;
using Domain.Entities.Users;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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
        });

        services.Configure<AuthOptions>(configuration.GetSection("Authentication"));
        services.Configure<SuperAdminOptions>(configuration.GetSection("SuperAdmin"));
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthenticationService>();

        services.AddHttpContextAccessor();

        services.AddHostedService<RecomendedGamesJob>();

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
