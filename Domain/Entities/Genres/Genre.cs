using Domain.Entities.Games;

namespace Domain.Entities.Genres;

public sealed class Genre
{
    public GenreId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public HashSet<Game> Games { get; private set; } = new();

    public static Genre Create(string name, string description)
        => new()
        {
            Name = name,
            Description = description
        };

    public void Update(string? name, string? description)
    {
        if (name is not null)
            Name = name;

        if (description is not null)
            Description = description;
    }
}
