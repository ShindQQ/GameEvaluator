﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Games.SetRating;

public sealed class SetRatingCommandHandler : IRequestHandler<SetRatingCommand>
{
    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    public SetRatingCommandHandler(
        IApplicationDbContext context,
        IUserRepository genreRepository)
    {
        _context = context;
        _userRepository = genreRepository;
    }

    public async Task Handle(SetRatingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var requestStatus = user.SetRatingToTheGame(request.GameId, request.Rating);

        if (!requestStatus)
            throw new NotFoundException(nameof(Game), request.GameId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
