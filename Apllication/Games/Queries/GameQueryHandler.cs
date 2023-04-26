using Apllication.Common.Interfaces.Repositories;
using Apllication.Common.Mappings;
using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Apllication.Games.Queries;

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
        var queryable = await _repository.GetAsync();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        return await queryable
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}