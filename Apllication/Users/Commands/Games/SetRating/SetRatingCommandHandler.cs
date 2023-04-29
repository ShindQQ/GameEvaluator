using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Users.Commands.Games.SetRating;

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
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.UserId);

        var requestStatus = user.SetRatingToTheGame(request.GameId, request.Rating);

        if (!requestStatus)
            throw new NotFoundException(nameof(Game), request.GameId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
