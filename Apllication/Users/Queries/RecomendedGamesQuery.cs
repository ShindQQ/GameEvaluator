using Apllication.Common.Models.DTOs;
using Domain.Entities.Users;
using MediatR;

namespace Apllication.Users.Queries;

public record RecomendedGamesQuery : IRequest<List<GameDto>>
{
    public UserId UserId { get; init; } = null!;

    public int AmountOfGames { get; init; }
}
