using Domain.Entities.Games;

namespace Domain.Entities.Jenres;

public sealed class Jenre
{
    public JenreId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public List<Game> Games { get; private set; } = new();
}
