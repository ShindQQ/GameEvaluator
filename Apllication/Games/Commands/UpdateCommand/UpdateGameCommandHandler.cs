using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

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
        var game = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Game with id {request.Id} was not found!");

        game.Update(request.Name, request.Description);

        await _repository.UpdateAsync(game, cancellationToken);
    }
}
