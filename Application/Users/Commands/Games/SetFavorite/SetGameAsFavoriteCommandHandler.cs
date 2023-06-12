using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Users.Commands.Games.SetFavorite;

public sealed class SetGameAsFavoriteCommandHandler : IRequestHandler<SetGameAsFavoriteCommand>
{
    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    public SetGameAsFavoriteCommandHandler(
        IApplicationDbContext context,
        IUserRepository genreRepository)
    {
        _context = context;
        _userRepository = genreRepository;
    }

    public async Task Handle(SetGameAsFavoriteCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.UserId} was not found!");

        var requestStatus = user.ChangeFavoriteState(request.GameId);

        if (!requestStatus)
            throw new StatusCodeException(HttpStatusCode.NotFound, $"Game with id {request.GameId} was not found!");

        await _context.SaveChangesAsync(cancellationToken);
    }
}
