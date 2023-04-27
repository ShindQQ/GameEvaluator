using Domain.Entities.Companies;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Companies.Commands.Games.CreateCommand;

public record CreateGameCommand : IRequest<GameId>
{
    public CompanyId? CompanyId { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }
}
