using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using MediatR;

namespace Application.Games.Commands.UpdateCommand;

public sealed class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand>
{
    private readonly IGameRepository _repository;

    public UpdateGameCommandHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (game == null)
            throw new NotFoundException(nameof(Game), request.Id);

        game.Update(request.Name, request.Description);

        await _repository.UpdateAsync(game, cancellationToken);
    }
}
