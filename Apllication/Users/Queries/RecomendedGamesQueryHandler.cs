using Apllication.Common.Interfaces.Repositories;
using Apllication.Common.Mappings;
using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Apllication.Users.Queries;

public class RecomendedGamesQueryHandler : IRequestHandler<RecomendedGamesQuery, PaginatedList<GameDto>>
{
    private readonly IUserRepository _repository;

    private readonly IMapper _mapper;

    public RecomendedGamesQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GameDto>> Handle(RecomendedGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await _repository.GetRecomendedGamesAsync(request.UserId, request.AmmountOfGames, cancellationToken);

        return await games
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize); 
    }
}
