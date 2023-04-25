using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Genres.RemoveGenre;

public sealed class RemoveGenreCommandHandler : IRequestHandler<RemoveGenreCommand>
{
    private readonly IGameRepository _repository;

    private readonly IApplicationDbContext _context;

    public RemoveGenreCommandHandler(IGameRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(RemoveGenreCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        game.RemoveGenre(request.GenreType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
