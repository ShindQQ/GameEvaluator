using Apllication.Common.Interfaces.Repositories;
using Apllication.Infrastructure;
using Domain.Entities.Users;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository
    : BaseRepository<User, UserId>,
    IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<User>> GetAsync()
        => Task.FromResult(Context.Users
            .AsQueryable());

    public async Task<User?> FindByEmailAsync(string email)
        => await Context.Users
        .Include(user => user.Company)
        .Include(user => user.Roles)
        .FirstOrDefaultAsync(user => user.Email.Equals(email));

    public async Task<User?> FindByNameAsync(string name)
        => await Context.Users
        .FirstOrDefaultAsync(user => user.Name.Equals(name));

    public async Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expirationTime)
    {
        user.SetRefreshToken(refreshToken, expirationTime);

        await Context.SaveChangesAsync();
    }

    public async Task GetRecomendedGamesAsync(UserId userId, int ammountOfGames)
    {
        var userGenres = Context.Users
            .Include(user => user.Games)
            .First(user => user.Id == userId)
            .Games
            .SelectMany(game => game.Game.Genres)
            .Distinct()
            .ToList();

        var games = Context.UserGame
            .Include(userGame => userGame.Game)
                .ThenInclude(game => game.Genres)
             .Where(userGame => userGame.UserId != userId && userGame.IsFavorite)
             .Select(userGame => userGame.Game)
             .GroupBy(game => game.Genres, game => game);

        Console.WriteLine();
    }

    public override async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        => await Context.Users
            .Include(user => user.Games)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
}
