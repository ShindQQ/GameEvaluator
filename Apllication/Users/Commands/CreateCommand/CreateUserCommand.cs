using Domain.Entities.Users;
using MediatR;

namespace Apllication.Users.Commands.CreateCommand;

public record CreateUserCommand : IRequest<UserId>
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? Password { get; init; }
}
