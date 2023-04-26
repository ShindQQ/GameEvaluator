using Domain.Entities.Games;
using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Games.Commands.Platforms.AddPlatform;

public record AddPlatformToGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public PlatformId PlatformId { get; init; } = null!;
}
