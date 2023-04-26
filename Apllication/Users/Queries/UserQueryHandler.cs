using Apllication.Common.Interfaces.Repositories;
using Apllication.Common.Mappings;
using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using Aplliction.Users.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Apllication.Users.Queries;

public sealed class UserQueryHandler : IRequestHandler<UserQuery, PaginatedList<UserDto>>
{
    private readonly IUserRepository _repository;

    private readonly IMapper _mapper;

    public UserQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<PaginatedList<UserDto>> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var queryable = await _repository.GetAsync();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        return await queryable
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
