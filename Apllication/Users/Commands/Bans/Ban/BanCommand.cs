using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Bans.Ban;

public record BanCommand : IRequest
{
    public UserId UserId { get; init; } = null!;

    public DateTime BanTo { get; init; }
}
