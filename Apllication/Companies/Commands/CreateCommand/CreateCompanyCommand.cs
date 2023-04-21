using Domain.Entities.Companies;
using MediatR;

namespace Apllication.Companies.Commands.CreateCommand;

public record CreateCompanyCommand : IRequest<CompanyId>
{
    public string? Name { get; init; }

    public string? Description { get; init; }
}
