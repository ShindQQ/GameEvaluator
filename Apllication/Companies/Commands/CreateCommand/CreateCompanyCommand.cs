using Domain.Entities.Companies;
using MediatR;

namespace Application.Companies.Commands.CreateCommand;

public record CreateCompanyCommand : IRequest<CompanyId>
{
    public string? Name { get; init; }

    public string? Description { get; init; }
}
