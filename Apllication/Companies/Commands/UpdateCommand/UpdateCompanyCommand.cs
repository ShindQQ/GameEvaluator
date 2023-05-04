using Domain.Entities.Companies;
using MediatR;

namespace Application.Companies.Commands.UpdateCommand;

public record UpdateCompanyCommand : IRequest
{
    public CompanyId Id { get; init; } = null!;

    public string? Name { get; init; }

    public string? Description { get; init; }
}
