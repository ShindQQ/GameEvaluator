﻿using Domain.Entities.Users;
using MediatR;

namespace Apllication.Users.Commands.UpdateCommand;

public record UpdateUserCommand : IRequest
{
    public UserId Id { get; init; } = null!;

    public string? Name { get; init; }

    public string? Email { get; init; }
}
