using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Platforms.AddPlatform;

public sealed class AddPlatformCommandHandler : IRequestHandler<AddPlatformCommand>
{
    private readonly IGameRepository _repository;

    private readonly IApplicationDbContext _context;

    public AddPlatformCommandHandler(IGameRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(AddPlatformCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NullReferenceException(nameof(game));

        game.AddPlatform(request.PlatformType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
