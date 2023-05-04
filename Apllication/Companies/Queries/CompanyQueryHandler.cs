using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Companies.Queries;

public sealed class CompanyQueryHandler : IRequestHandler<CompanyQuery, PaginatedList<CompanyDto>>
{
    private readonly ICompanyRepository _repository;

    private readonly IMapper _mapper;

    public CompanyQueryHandler(ICompanyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CompanyDto>> Handle(CompanyQuery request, CancellationToken cancellationToken)
    {
        var queryable = await _repository.GetAsync();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        return await queryable
            .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}