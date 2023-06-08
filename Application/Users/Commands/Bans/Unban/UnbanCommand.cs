using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Bans.Unban;

public record UnbanCommand : IRequest
{
    public UserId UserId { get; init; } = null!;
}
