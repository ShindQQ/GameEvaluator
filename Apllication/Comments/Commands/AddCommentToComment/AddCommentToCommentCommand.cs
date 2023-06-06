using Domain.Entities.Comments;
using Domain.Entities.Users;
using MediatR;

namespace Application.Comments.Commands.AddCommentToComment;

public record AddCommentToCommentCommand : IRequest<CommentId>
{
    public CommentId ParrentCommentId { get; init; } = null!;

    public UserId UserId { get; init; } = null!;

    public string Text { get; init; } = string.Empty;
}
