using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Apllication.Users.Commands.Games.SetRating;

public record SetRatingCommand : IRequest
{
    public UserId UserId { get; init; } = null!;

    public GameId GameId { get; init; } = null!;

    public int Rating { get; init; }
}
