using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Apllication.Companies.Queries;

public sealed class CompanyQueryHandler : IRequestHandler<CompanyQuery, List<Company>>
{
    private readonly ICompanyRepository _repository;

    public CompanyQueryHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Company>> Handle(CompanyQuery request, CancellationToken cancellationToken)
        => await (await _repository.GetAsync()).ToListAsync(cancellationToken);
}