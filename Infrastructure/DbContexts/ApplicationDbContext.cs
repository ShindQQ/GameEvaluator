using Apllication.Common.Interfaces;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Intermidiate;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Game> Games => base.Set<Game>();

    public DbSet<UserGame> UserGames => Set<UserGame>();

    public DbSet<User> Users => base.Set<User>();
}
