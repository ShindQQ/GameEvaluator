namespace Application.Common.Models.Tokens;

public record TokenModel
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTime Expiration { get; init; }

    public bool IsBanned { get; init; }

    public bool PasswordIncorrect { get; init; }
}
