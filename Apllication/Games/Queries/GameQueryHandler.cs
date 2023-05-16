using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Games.Queries;

public sealed class GameQueryHandler : IRequestHandler<GameQuery, PaginatedList<GameDto>>
{
    private readonly IGameRepository _repository;

    private readonly IMapper _mapper;

    public GameQueryHandler(IGameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GameDto>> Handle(GameQuery request, CancellationToken cancellationToken)
    {
        var queryable = (await _repository.GetAsync())
            .Include(game => game.Comments)
            .ThenInclude(comment => comment.ChildrenComments)
            .AsQueryable();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        return await queryable
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}