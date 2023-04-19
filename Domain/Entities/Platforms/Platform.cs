using Domain.Entities.Games;

namespace Domain.Entities.Platforms;

public sealed class Platform
{
    public PlatformId Id { get; private set; } = null!;

    public PlatformType Name { get; private set; }

    public HashSet<Game> Games { get; private set; } = new();
}

public enum PlatformType
{
    Pc,
    Xbox,
    PlayStation,
    Nintendo,
    Other
}
