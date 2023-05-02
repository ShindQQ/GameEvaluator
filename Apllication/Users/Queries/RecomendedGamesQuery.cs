using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using Domain.Entities.Users;
using MediatR;

namespace Apllication.Users.Queries;

public record RecomendedGamesQuery : IRequest<PaginatedList<GameDto>>
{
    public UserId UserId { get; init; } = null!;

    public int AmmountOfGames { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
