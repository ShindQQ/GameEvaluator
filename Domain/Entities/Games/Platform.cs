namespace Domain.Entities.Games;

public record Platform
{
    private Platform(string name) => Name = name;

    public string Name { get; init; }

    public static Platform? Create(PlatformType platformType)
    {
        return new Platform(platformType.ToString());
    }
}

public enum PlatformType
{
    Pc,
    Xbox,
    PlayStation,
    Nintendo,
    Other
}
