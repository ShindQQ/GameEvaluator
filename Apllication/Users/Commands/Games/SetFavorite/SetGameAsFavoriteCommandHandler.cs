using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Games.SetFavorite;

public sealed class SetGameAsFavoriteCommandHandler : IRequestHandler<SetGameAsFavoriteCommand>
{
    private readonly IGameRepository _gameRepository;

    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    public SetGameAsFavoriteCommandHandler(
        IGameRepository gameRepository,
        IApplicationDbContext context,
        IUserRepository genreRepository)
    {
        _gameRepository = gameRepository;
        _context = context;
        _userRepository = genreRepository;
    }

    public async Task Handle(SetGameAsFavoriteCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var requestStatus = user.ChangeFavoriteState(request.GameId);

        if (!requestStatus)
            throw new NotFoundException(nameof(Game), request.GameId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
