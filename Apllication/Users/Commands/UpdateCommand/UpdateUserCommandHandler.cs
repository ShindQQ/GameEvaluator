using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Users.Commands.UpdateCommand;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken)
           ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.Id} was not found!");

        user.Update(request.Name, request.Email);

        await _repository.UpdateAsync(user, cancellationToken);
    }
}
