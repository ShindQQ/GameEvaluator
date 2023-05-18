namespace Application.Common.Requests;

public sealed class AddCommentToCommentRequest
{
    public Guid CommentId { get; init; }

    public Guid GameId { get; init; }

    public string Text { get; init; } = string.Empty;
}
