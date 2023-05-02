using Apllication.Common.Interfaces.Repositories;
using Apllication.Infrastructure;
using Domain.Entities.Games;
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

    public async Task<IQueryable<Game>> GetRecomendedGamesAsync(
        UserId userId,
        int ammountOfGames,
        CancellationToken cancellationToken)
    {
        var gameIds = await Context.UserGame
            .Where(userGame => userGame.UserId != userId)
            .Where(userGame => userGame.IsFavorite)
            .GroupBy(userGame => userGame.GameId)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .ToListAsync(cancellationToken);

        var userGames = await Context.UserGame
            .Where(userGame => userGame.UserId == userId)
            .Select(userGame => userGame.GameId)
            .ToListAsync(cancellationToken);

        var result = Context.Games
            .Include(game => game.Genres)
            .Include(game => game.Companies)
            .Include(game => game.Platforms)
            .Where(game => gameIds.Contains(game.Id))
            .Where(game => !userGames.Contains(game.Id))
            .Take(ammountOfGames);

        return result;

        //var user = await GetByIdAsync(userId, cancellationToken);

        //var smth = await Context.UserGame
        //    .Include(userGame => userGame.User)
        //    .Include(userGame => userGame.Game.Genres)
        //    .Include(userGame => userGame.Game.Companies)
        //    .Include(userGame => userGame.Game.GameUsers)
        //    .Include(userGame => userGame.Game.Platforms)
        //    .Where(userGame => userGame.IsFavorite)
        //    .Where(userGame => userGame.UserId != userId)
        //    .OrderBy(userGame => userGame.Game.GameUsers.Count(userGame => userGame.IsFavorite))
        //    .Select(userGame => userGame.Game)
        //    .ToListAsync(cancellationToken);

        //return smth.Except(user!.Games.Select(userGame => userGame.Game).ToList()).ToList();
    }

    public override async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        => await Context.Users
            .Include(user => user.Games)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
}
