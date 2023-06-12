namespace Presentration.API.Options;

public record AuthOptions
{
    public string SecretForKey { get; set; } = string.Empty;
}
