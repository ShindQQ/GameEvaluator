﻿using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Users.Commands.Roles.AddRole;

public sealed class AddRoleCommandHandler : IRequestHandler<AddRoleCommand>
{
    private readonly IUserRepository _repository;

    private readonly IApplicationDbContext _context;

    public AddRoleCommandHandler(IUserRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new NullReferenceException(nameof(user));

        user.AddRole(request.RoleType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
