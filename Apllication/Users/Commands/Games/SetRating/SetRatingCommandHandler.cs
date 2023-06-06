using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

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
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.UserId} was not found!");

        var requestStatus = user.SetRatingToTheGame(request.GameId, request.Rating);

        if (!requestStatus)
            throw new StatusCodeException(HttpStatusCode.NotFound, $"Game with id {request.GameId} was not found!");

        await _context.SaveChangesAsync(cancellationToken);
    }
}
