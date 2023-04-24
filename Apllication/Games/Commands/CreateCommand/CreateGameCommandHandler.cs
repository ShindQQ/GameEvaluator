using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Commands.CreateCommand;

public sealed class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameId>
{
    private readonly IGameRepository _repository;

    public CreateGameCommandHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<GameId> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var game = Game.Create(request.Name!, request.Description!);

        await _repository.AddAsync(game, cancellationToken);

        return game.Id;
    }
}
