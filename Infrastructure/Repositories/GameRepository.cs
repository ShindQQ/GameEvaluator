﻿using Apllication.Common.Interfaces.Repositories;
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
        => Task.FromResult(Context.Games
            .Include(game => game.Companies)
            .Include(game => game.Genres)
            .Include(game => game.Platforms)
            .AsQueryable());

    public override async Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken)
        => await Context.Games
            .Include(game => game.Companies)
            .Include(game => game.Genres)
            .Include(game => game.Platforms)
            .FirstOrDefaultAsync(game => game.Id == id, cancellationToken);
}
