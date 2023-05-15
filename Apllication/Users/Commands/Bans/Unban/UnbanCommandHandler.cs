using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Users;
using MediatR;

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
            ?? throw new NotFoundException(nameof(User), request.UserId);

        user.UnBan();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
