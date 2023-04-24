using Apllication.Common.Interfaces.Repositories;
using Apllication.Infrastructure;
using Domain.Entities.Games;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class GameRepository
    : BaseRepository<Game, GameId>,
    IGameRepository
{
    public GameRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<Game>> GetAsync()
        => Task.FromResult(Context.Game
            .AsQueryable());

    public override async Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken)
        => await Context.Game
            .FirstOrDefaultAsync(game => game.Id == id, cancellationToken);
}
