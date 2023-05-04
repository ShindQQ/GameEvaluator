using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using MediatR;

namespace Application.Companies.Commands.UpdateCommand;

public sealed class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly ICompanyRepository _repository;

    public UpdateCompanyCommandHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (company == null)
            throw new NotFoundException(nameof(Company), request.Id);

        company.Update(request.Name, request.Description);

        await _repository.UpdateAsync(company, cancellationToken);
    }
}
