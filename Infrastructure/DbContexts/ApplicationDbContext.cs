using Application.Common.Interfaces;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using Domain.Entities.Intermidiate;
using Domain.Entities.Platforms;
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

    public DbSet<Game> Games => Set<Game>();

    public DbSet<UserGame> UserGame => Set<UserGame>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<Platform> Platforms => Set<Platform>();
}
