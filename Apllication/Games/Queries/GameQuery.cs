using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Queries;

public record GameQuery : IRequest<PaginatedList<GameDto>>
{
    public GameId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
