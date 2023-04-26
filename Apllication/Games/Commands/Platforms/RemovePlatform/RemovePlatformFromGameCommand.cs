using Domain.Entities.Games;
using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Games.Commands.Platforms.RemovePlatform;

public record RemovePlatformFromGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public PlatformId PlatformId { get; init; } = null!;
}
