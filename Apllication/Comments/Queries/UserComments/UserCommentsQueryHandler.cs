using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Comments.Queries.UserComments;

public sealed class UserCommentsQueryHandler : IRequestHandler<UserCommentsQuery, PaginatedList<CommentDto>>
{
    private readonly IUserRepository _repository;

    private readonly IMapper _mapper;

    public UserCommentsQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CommentDto>> Handle(UserCommentsQuery request, CancellationToken cancellationToken)
    {
        var queryable = (await _repository.GetAsync())
            .Where(entity => entity.Id == request.UserId)
            .SelectMany(entity => entity.Comments);

        return await queryable
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
