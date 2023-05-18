using Domain.Entities.Comments;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Games.Commands.Comments.AddComment;

public record AddCommentCommand : IRequest<CommentId>
{
    public GameId GameId { get; init; } = null!;

    public UserId? UserId { get; init; }

    public string Text { get; init; } = string.Empty;
}
