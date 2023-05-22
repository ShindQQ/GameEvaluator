using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Comments.Queries.GameComments;

public sealed class GameCommentsQueryHandler : IRequestHandler<GameCommentsQuery, PaginatedList<CommentDto>>
{
    private readonly IGameRepository _repository;

    private readonly IMapper _mapper;

    public GameCommentsQueryHandler(IGameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CommentDto>> Handle(GameCommentsQuery request, CancellationToken cancellationToken)
    {
        var queryable = (await _repository.GetAsync())
            .Where(entity => entity.Id == request.GameId)
            .SelectMany(entity => entity.Comments);

        return await queryable
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
