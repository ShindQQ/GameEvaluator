using Domain.Entities.Users;
using Domain.Enums;
using MediatR;

namespace Application.Users.Commands.Roles.AddRole;

public record AddRoleCommand : IRequest
{
    public UserId UserId { get; init; } = null!;

    public RoleType RoleType { get; init; }
}
