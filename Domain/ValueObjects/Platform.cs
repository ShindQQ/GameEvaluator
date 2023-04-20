using Domain.Enums;

namespace Domain.ValueObjects;

public record Platform
{
    private Platform(string name) => Name = name;

    public string Name { get; init; }

    public static Platform? Create(PlatformType platformType)
    {
        return new Platform(platformType.ToString());
    }
}
