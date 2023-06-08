using Domain.Entities.Companies;
using Domain.Entities.Games;
using MediatR;

namespace Application.Companies.Commands.Games.RemoveGame;

public record RemoveGameFromCompanyCommand : IRequest
{
    public CompanyId? CompanyId { get; init; }

    public GameId GameId { get; init; } = null!;
}
