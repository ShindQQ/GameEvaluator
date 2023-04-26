using Apllication.Common.Exceptions;
using Apllication.Common.Interface;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Platforms.AddPlatform;

public sealed class AddPlatformCommandToGameHandler : IRequestHandler<AddPlatformToGameCommand>
{
    private readonly IGameRepository _gameRepository;

    private readonly IPlatformRepository _platformRepository;

    private readonly IApplicationDbContext _context;

    public AddPlatformCommandToGameHandler(
        IGameRepository gameRepository,
        IApplicationDbContext context,
        IPlatformRepository platformRepository)
    {
        _gameRepository = gameRepository;
        _context = context;
        _platformRepository = platformRepository;
    }

    public async Task Handle(AddPlatformToGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        var platform = await _platformRepository.GetByIdAsync(request.PlatformId, cancellationToken);

        if (platform is null)
            throw new NotFoundException(nameof(platform), request.PlatformId);

        game.AddPlatform(platform);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
