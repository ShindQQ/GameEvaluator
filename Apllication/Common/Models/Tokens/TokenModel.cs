namespace Apllication.Common.Models.Tokens;

public record TokenModel
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTime Expiration { get; init; }
}
