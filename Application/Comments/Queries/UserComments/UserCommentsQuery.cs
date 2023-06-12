using Application.Common.Models;
using Application.Common.Models.DTOs;
using Domain.Entities.Users;
using MediatR;

namespace Application.Comments.Queries.UserComments;

public record UserCommentsQuery : IRequest<PaginatedList<CommentDto>>
{
    public UserId UserId { get; init; } = null!;

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
