using Domain.Entities.Comments;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using Domain.Entities.Intermidiate;
using Domain.Entities.Platforms;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Company> Companies { get; }

    public DbSet<Game> Games { get; }

    public DbSet<UserGame> UserGame { get; }

    public DbSet<User> Users { get; }

    public DbSet<Genre> Genres { get; }

    public DbSet<Platform> Platforms { get; }

    public DbSet<Comment> Comments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
