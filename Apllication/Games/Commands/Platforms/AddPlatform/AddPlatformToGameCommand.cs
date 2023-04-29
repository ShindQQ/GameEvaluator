using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Games.Commands.Platforms.AddPlatform;

public record AddPlatformToGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public CompanyId? CompanyId { get; init; }

    public PlatformId PlatformId { get; init; } = null!;
}
