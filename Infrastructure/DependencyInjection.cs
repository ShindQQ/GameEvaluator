using Application.Common.Interface;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
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
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();

        return services;
    }
}
