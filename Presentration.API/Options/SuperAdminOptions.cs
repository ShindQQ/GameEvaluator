namespace Presentration.API.Options;

public record SuperAdminOptions
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
