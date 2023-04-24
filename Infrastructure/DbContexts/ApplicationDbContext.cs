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

    public DbSet<Company> Company => Set<Company>();

    public DbSet<Game> Game => base.Set<Game>();

    public DbSet<UserGame> UserGame => Set<UserGame>();

    public DbSet<User> User => base.Set<User>();
}
