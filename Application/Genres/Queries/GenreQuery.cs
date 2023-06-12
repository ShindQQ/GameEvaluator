using Application.Common.Models;
using Application.Common.Models.DTOs;
using Domain.Entities.Genres;
using MediatR;

namespace Application.Genres.Queries;

public record GenreQuery : IRequest<PaginatedList<GenreDto>>
{
    public GenreId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
