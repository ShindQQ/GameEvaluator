using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.DeleteCommand;

public record DeleteUserCommand(UserId Id) : IRequest;
