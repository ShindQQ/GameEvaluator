using Domain.Entities.Users;
using MediatR;

namespace Apllication.Users.Commands.DeleteCommand;

public record DeleteUserCommand(UserId Id) : IRequest;
