using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Users.Commands.DeleteCommand;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.Id} was not found!");

        await _repository.DeleteAsync(user, cancellationToken);
    }
}
