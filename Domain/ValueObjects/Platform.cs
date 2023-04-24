using Domain.Enums;

namespace Domain.ValueObjects;

public record Platform
{
    public static Platform Create(PlatformType platformType) => new() { Name = platformType.ToString() };

    public string Name { get; init; } = string.Empty;
}
