using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Games.AddGame;

public sealed class AddGameToUserCommandHandler : IRequestHandler<AddGameToUserCommand>
{
    private readonly IGameRepository _gameRepository;

    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    public AddGameToUserCommandHandler(
        IGameRepository gameRepository,
        IApplicationDbContext context,
        IUserRepository genreRepository)
    {
        _gameRepository = gameRepository;
        _context = context;
        _userRepository = genreRepository;
    }

    public async Task Handle(AddGameToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken)
            ?? throw new NotFoundException(nameof(Game), request.GameId);

        user.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
