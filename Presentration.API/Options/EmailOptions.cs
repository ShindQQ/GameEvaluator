namespace Presentration.API.Options;

public record EmailOptions
{
    public string Host { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
