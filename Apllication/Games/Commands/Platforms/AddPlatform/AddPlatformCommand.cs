using Domain.Entities.Games;
using Domain.Enums;
using MediatR;

namespace Apllication.Games.Commands.Platforms.AddPlatform;

public record AddPlatformCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public PlatformType PlatformType { get; init; }
}
