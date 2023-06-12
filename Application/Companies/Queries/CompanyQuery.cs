using Application.Common.Models;
using Application.Common.Models.DTOs;
using Domain.Entities.Companies;
using MediatR;

namespace Application.Companies.Queries;

public record CompanyQuery : IRequest<PaginatedList<CompanyDto>>
{
    public CompanyId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
