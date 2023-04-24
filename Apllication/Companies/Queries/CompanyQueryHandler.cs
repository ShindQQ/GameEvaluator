using Apllication.Common.Interfaces.Repositories;
using Apllication.Common.Mappings;
using Apllication.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Apllication.Companies.Queries;

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