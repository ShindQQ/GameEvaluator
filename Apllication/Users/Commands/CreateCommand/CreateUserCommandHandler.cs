using Application.Common.Interfaces.Repositories;
using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.CreateCommand;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserId>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserId> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var game = User.Create(request.Name!, request.Email!, request.Password!);

        await _repository.AddAsync(game, cancellationToken);

        return game.Id;
    }
}
