using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;
using System.Net;

namespace Application.Users.Commands.Bans.Ban;

public sealed class BanCommandHandler : IRequestHandler<BanCommand>
{
    private readonly IUserRepository _repository;

    private readonly IApplicationDbContext _context;

    public BanCommandHandler(IUserRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(BanCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.UserId} was not found!");

        var roles = user.Roles.Where(role => role.Name.Equals(RoleType.SuperAdmin) || role.Name.Equals(RoleType.Admin));

        if (roles.Any())
        {
            return;
        }

        user.Ban(request.BanTo);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
