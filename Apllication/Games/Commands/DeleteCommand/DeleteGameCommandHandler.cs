using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using MediatR;

namespace Application.Games.Commands.DeleteCommand;

public sealed class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand>
{
    private readonly IGameRepository _repository;

    public DeleteGameCommandHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Game), request.Id);

        await _repository.DeleteAsync(game, cancellationToken);
    }
}
