using Apllication.Common.Interface;
using Apllication.Common.Mappings;
using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Apllication.Platforms.Queries;

public sealed class PlatformQueryHandler : IRequestHandler<PlatformQuery, PaginatedList<PlatformDto>>
{
    private readonly IPlatformRepository _repository;

    private readonly IMapper _mapper;

    public PlatformQueryHandler(IPlatformRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlatformDto>> Handle(PlatformQuery request, CancellationToken cancellationToken)
    {
        var queryable = await _repository.GetAsync();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        return await queryable
            .ProjectTo<PlatformDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
