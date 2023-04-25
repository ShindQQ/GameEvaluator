using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Platforms.RemovePlatform;

public sealed class RemovePlatformCommandHandler : IRequestHandler<RemovePlatformCommand>
{
    private readonly IGameRepository _repository;

    private readonly IApplicationDbContext _context;

    public RemovePlatformCommandHandler(IGameRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(RemovePlatformCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        game.RemovePlatform(request.PlatformType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
