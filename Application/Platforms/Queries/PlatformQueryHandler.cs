using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Platforms.Queries;

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
