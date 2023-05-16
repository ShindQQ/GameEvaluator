namespace Application.Common.Models.DTOs;

public sealed class CommentDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = string.Empty;

    public Guid GameId { get; init; }

    public Guid? ParentCommentId { get; init; }

    public string LeftBy { get; init; } = string.Empty;

    public List<CommentDto> ChildrenComments { get; init; } = new();
}
