using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Genres.AddGenre;

public sealed class AddGenreToGameCommandHandler : IRequestHandler<AddGenreToGameCommand>
{
    private readonly IGameRepository _gameRepository;

    private readonly IGenreRepository _genreRepository;

    private readonly IApplicationDbContext _context;

    public AddGenreToGameCommandHandler(
        IGameRepository gameRepository,
        IApplicationDbContext context,
        IGenreRepository genreRepository)
    {
        _gameRepository = gameRepository;
        _context = context;
        _genreRepository = genreRepository;
    }

    public async Task Handle(AddGenreToGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        var genre = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken);

        if (genre is null)
            throw new NotFoundException(nameof(genre), request.GenreId);

        game.AddGenre(genre);

        await _context.SaveChangesAsync(cancellationToken);
    }
}