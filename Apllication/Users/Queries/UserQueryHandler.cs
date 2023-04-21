using Apllication.Common.Interfaces.Repositories;
using Aplliction.Users.Queries;
using Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Apllication.Users.Queries;

public sealed class UserQueryHandler : IRequestHandler<UserQuery, List<User>>
{
    private readonly IUserRepository _repository;

    public UserQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<User>> Handle(UserQuery request, CancellationToken cancellationToken)
        => await (await _repository.GetAsync()).ToListAsync(cancellationToken);
}
