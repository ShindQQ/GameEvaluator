using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Commands.UpdateCommand;

public record UpdateGameCommand : IRequest
{
    public GameId Id { get; init; } = null!;

    public string? Name { get; init; }

    public string? Description { get; init; }
}
