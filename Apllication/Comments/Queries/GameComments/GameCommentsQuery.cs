using Application.Common.Models;
using Application.Common.Models.DTOs;
using Domain.Entities.Games;
using MediatR;

namespace Application.Comments.Queries.GameComments;

public record GameCommentsQuery : IRequest<PaginatedList<CommentDto>>
{
    public GameId GameId { get; init; } = null!;

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
