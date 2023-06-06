using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Users.Commands.Bans.Unban;

public sealed class UnbanCommandHandler : IRequestHandler<UnbanCommand>
{
    private readonly IUserRepository _repository;

    private readonly IApplicationDbContext _context;

    public UnbanCommandHandler(IUserRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(UnbanCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.UserId} was not found!");

        if (user.BanState is null)
            throw new StatusCodeException(HttpStatusCode.BadRequest, $"User {request.UserId} is not banned!");

        user.UnBan();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
