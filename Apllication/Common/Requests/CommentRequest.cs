using Domain.Entities.Users;

namespace Application.Common.Requests;

public record CommentRequest
{
    public UserId UserId { get; init; } = null!;

    public string Text { get; init; } = string.Empty;
}
