namespace Apllication.Common.Requests;

public record CreateGameRequest
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
