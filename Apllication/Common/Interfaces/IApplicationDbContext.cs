using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Intermidiate;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Apllication.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Company> Company { get; }

    public DbSet<Game> Game { get; }

    public DbSet<UserGame> UserGame { get; }

    public DbSet<User> User { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
