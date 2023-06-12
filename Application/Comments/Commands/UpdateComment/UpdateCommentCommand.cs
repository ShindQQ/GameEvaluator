using Domain.Entities.Comments;
using Domain.Entities.Users;
using MediatR;

namespace Application.Comments.Commands.UpdateComment;

public record UpdateCommentCommand : IRequest
{
    public CommentId Id { get; init; } = null!;

    public UserId UserId { get; init; } = null!;

    public string Text { get; init; } = string.Empty;
}
