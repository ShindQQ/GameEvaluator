using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Platforms.Queries;

public sealed class PlatformQuery : IRequest<PaginatedList<PlatformDto>>
{
    public PlatformId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
