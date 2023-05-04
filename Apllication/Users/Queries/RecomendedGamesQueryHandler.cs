using Application.Common.Interfaces;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public class RecomendedGamesQueryHandler : IRequestHandler<RecomendedGamesQuery, List<GameDto>>
{
    private readonly IApplicationDbContext _context;

    private readonly IMapper _mapper;

    public RecomendedGamesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GameDto>> Handle(RecomendedGamesQuery request, CancellationToken cancellationToken)
    {
        var games = _context.Games
            .Where(game => _context.UserGame
                .Where(userGame => userGame.UserId != request.UserId)
                .Where(userGame => userGame.IsFavorite)
                .GroupBy(userGame => userGame.GameId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .Contains(game.Id))
            .Where(game => !_context.UserGame
                .Where(userGame => userGame.UserId == request.UserId)
                .Select(userGame => userGame.GameId)
                .Contains(game.Id))
            .Take(request.AmountOfGames);

        return await games
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
