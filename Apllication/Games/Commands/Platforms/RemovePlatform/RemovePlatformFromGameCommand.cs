using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Games.Commands.Platforms.RemovePlatform;

public record RemovePlatformFromGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public CompanyId? CompanyId { get; init; }

    public PlatformId PlatformId { get; init; } = null!;
}
