using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Users.Commands.Roles.RemoveRole;

public sealed class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand>
{
    private readonly IUserRepository _repository;

    private readonly IApplicationDbContext _context;

    public RemoveRoleCommandHandler(IUserRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.UserId} was not found!");

        user.RemoveRole(request.RoleType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
