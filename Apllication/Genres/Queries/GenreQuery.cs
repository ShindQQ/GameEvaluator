using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Genres.Queries;

public record GenreQuery : IRequest<PaginatedList<GenreDto>>
{
    public GenreId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
