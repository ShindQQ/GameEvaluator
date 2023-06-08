using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

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
        var company = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Company with id {request.Id} was not found!");

        await _repository.DeleteAsync(company, cancellationToken);
    }
}
