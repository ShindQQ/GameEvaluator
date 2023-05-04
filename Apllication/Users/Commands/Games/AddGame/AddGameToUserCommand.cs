using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Games.AddGame;

public sealed record AddGameToUserCommand : IRequest
{
    public UserId UserId { get; init; } = null!;

    public GameId GameId { get; init; } = null!;
}
