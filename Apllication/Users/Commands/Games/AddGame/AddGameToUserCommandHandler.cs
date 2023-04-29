﻿using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Users.Commands.Games.AddGame;

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
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.UserId);

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        user.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
