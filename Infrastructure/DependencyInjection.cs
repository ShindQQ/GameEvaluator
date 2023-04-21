using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Cached;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructre;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.Decorate<IUserRepository, CachedUserRepository>();

        services.AddScoped<IGameRepository, GameRepository>();
        services.Decorate<IGameRepository, CachedGameRepository>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.Decorate<ICompanyRepository, CachedCompanyRepository>();

        return services;
    }
}
