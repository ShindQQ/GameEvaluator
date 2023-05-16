using Domain.Entities.Comments;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Games.Commands.Comments.AddCommentToComment;

public record AddCommentToCommentCommand : IRequest
{
    public CommentId ParrentCommentId { get; init; } = null!;

    public GameId GameId { get; init; } = null!;

    public UserId UserId { get; init; } = null!;

    public string Text { get; init; } = string.Empty;
}
