namespace Application.Common.Models.Tokens;

public record RefreshTokenModel
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;
}
