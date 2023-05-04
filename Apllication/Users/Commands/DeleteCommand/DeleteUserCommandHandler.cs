using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Users;
using MediatR;

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
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);

        await _repository.DeleteAsync(user, cancellationToken);
    }
}
