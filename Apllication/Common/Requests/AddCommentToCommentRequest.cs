namespace Application.Common.Requests;

public record AddCommentToCommentRequest
{
    public Guid CommentId { get; init; }

    public Guid GameId { get; init; }

    public string Text { get; init; } = string.Empty;
}
