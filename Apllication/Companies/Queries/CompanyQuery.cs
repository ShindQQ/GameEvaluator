using Apllication.Common.Models;
using Domain.Entities.Companies;
using MediatR;

namespace Apllication.Companies.Queries;

public record CompanyQuery : IRequest<PaginatedList<CompanyDto>>
{
    public CompanyId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
