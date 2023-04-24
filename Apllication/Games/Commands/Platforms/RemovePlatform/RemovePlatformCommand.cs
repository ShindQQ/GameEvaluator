using Domain.Entities.Games;
using Domain.Enums;
using MediatR;

namespace Apllication.Games.Commands.Platforms.RemovePlatform;

public record RemovePlatformCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public PlatformType PlatformType { get; init; }
}
