using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Games.Commands.Genres.AddGenre;

public sealed class AddGenreCommandHandler : IRequestHandler<AddGenreCommand>
{
    private readonly IGameRepository _repository;

    private readonly IApplicationDbContext _context;

    public AddGenreCommandHandler(IGameRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(AddGenreCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        game.AddGenre(request.GenreType);

        await _context.SaveChangesAsync(cancellationToken);
    }
}