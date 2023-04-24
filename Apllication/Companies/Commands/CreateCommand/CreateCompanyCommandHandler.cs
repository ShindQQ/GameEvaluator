using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using MediatR;

namespace Apllication.Companies.Commands.CreateCommand;

public sealed class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyId>
{
    private readonly ICompanyRepository _repository;

    public CreateCompanyCommandHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<CompanyId> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = Company.Create(request.Name!, request.Description!);

        await _repository.AddAsync(company, cancellationToken);

        return company.Id;
    }
}
