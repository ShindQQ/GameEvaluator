﻿using Domain.Entities.Users;
using Domain.Enums;
using MediatR;

namespace Apllication.Users.Commands.Roles.RemoveRole;

public record RemoveRoleCommand : IRequest
{
    public UserId UserId { get; init; } = null!;

    public RoleType RoleType { get; init; }
}
