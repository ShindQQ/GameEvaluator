using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Intermidiate;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Apllication.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Company> Companies { get; }

    public DbSet<Game> Games { get; }

    public DbSet<UserGame> UserGames { get; }

    public DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
