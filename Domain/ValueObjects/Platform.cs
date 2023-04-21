using Domain.Enums;

namespace Domain.ValueObjects;

public record Platform
{
    public Platform(PlatformType platformType) => Name = platformType.ToString();

    public string Name { get; init; }
}
