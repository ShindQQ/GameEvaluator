using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.Roles.AddRole;

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
            throw new NotFoundException(nameof(user), request.UserId);

        user.AddRole(request.RoleType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
