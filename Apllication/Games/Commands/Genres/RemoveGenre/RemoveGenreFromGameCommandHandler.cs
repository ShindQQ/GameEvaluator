using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Genres.RemoveGenre;

public sealed class RemoveGenreFromGameCommandHandler : IRequestHandler<RemoveGenreFromGameCommand>
{
    private readonly IGameRepository _gameRepository;

    private readonly IGenreRepository _genreRepository;

    private readonly IApplicationDbContext _context;

    public RemoveGenreFromGameCommandHandler(
        IGameRepository gameRepository,
        IApplicationDbContext context,
        IGenreRepository genreRepository)
    {
        _gameRepository = gameRepository;
        _context = context;
        _genreRepository = genreRepository;
    }

    public async Task Handle(RemoveGenreFromGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        var genre = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken);

        if (genre is null)
            throw new NotFoundException(nameof(genre), request.GenreId);

        game.RemoveGenre(genre);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
