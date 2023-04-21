using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Apllication.Games.Queries;

public sealed class GameQueryHandler : IRequestHandler<GameQuery, List<Game>>
{
    private readonly IGameRepository _repository;

    public GameQueryHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Game>> Handle(GameQuery request, CancellationToken cancellationToken)
        => await (await _repository.GetAsync()).ToListAsync(cancellationToken);
}