using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using MediatR;

namespace Application.Companies.Commands.DeleteCommand;

public sealed class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly ICompanyRepository _repository;

    public DeleteCompanyCommandHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (company == null)
            throw new NotFoundException(nameof(Company), request.Id);

        await _repository.DeleteAsync(company, cancellationToken);
    }
}
