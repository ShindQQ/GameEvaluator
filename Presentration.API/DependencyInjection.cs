using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presentration.API.Options;
using Presentration.API.Services;
using System.Text;

namespace Presentration.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
        {
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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthenticationService>();

            services.AddHttpContextAccessor();

            return services;
        }

        public static async Task AddSuperAdminRole(this IServiceScope scope)
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var admin = scope.ServiceProvider.GetRequiredService<IOptions<SuperAdminOptions>>().Value;

            if (await userRepository.FindByEmailAsync(admin.Email) is null)
            {
                var adminUser = User.Create(admin.Email, admin.Email, admin.Password);
                adminUser.AddRole("SuperAdmin");

                await userRepository.AddAsync(adminUser);
            }
        }
    }
}
