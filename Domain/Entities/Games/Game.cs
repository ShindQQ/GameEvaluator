using Domain.Entities.Companies;
using Domain.Entities.Genres;
using Domain.Entities.Intermidiate;
using Domain.Entities.Platforms;

namespace Domain.Entities.Games;

public sealed class Game
{
    public GameId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public HashSet<Genre> Genres { get; private set; } = new();

    public HashSet<Company> Companies { get; private set; } = new();

    public HashSet<UserGame> GameUsers { get; private set; } = new();

    public HashSet<Platform> Platforms { get; private set; } = new();

    public static Game Create(string name, string description)
        => new()
        {
            Id = new GameId(Guid.NewGuid()),
            Name = name,
            Description = description,
        };

    public void Update(string? name, string? description)
    {
        if (name is not null)
            Name = name;

        if (description is not null)
            Description = description;
    }

    public bool AddPlatform(Platform platform)
        => Platforms.Add(platform);

    public bool RemovePlatform(Platform platform)
    {
        var foundPlatform = Platforms.FirstOrDefault(platformEntity => platformEntity.Id == platform.Id);

        if (foundPlatform is null)
            return false;

        Platforms.Remove(foundPlatform);

        return true;
    }

    public bool AddGenre(Genre genre)
        => Genres.Add(genre);

    public bool RemoveGenre(Genre genre)
    {
        var foundGenre = Genres.FirstOrDefault(genreEntity => genre.Id == genreEntity.Id);

        if (foundGenre is null)
            return false;

        Genres.Remove(foundGenre);

        return true;
    }
}
