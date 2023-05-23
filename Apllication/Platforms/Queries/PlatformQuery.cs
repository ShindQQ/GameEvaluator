using Application.Common.Models;
using Application.Common.Models.DTOs;
using Domain.Entities.Platforms;
using MediatR;

namespace Application.Platforms.Queries;

public record PlatformQuery : IRequest<PaginatedList<PlatformDto>>
{
    public PlatformId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
