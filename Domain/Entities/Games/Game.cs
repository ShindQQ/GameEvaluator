using Domain.Entities.Companies;
using Domain.Entities.Intermidiate;
using Domain.Enums;
using Domain.ValueObjects;

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

    public bool AddPlatform(PlatformType platformType)
        => Platforms.Add(Platform.Create(platformType));

    public bool RemovePlatform(PlatformType platformType)
    {
        var platform = Platforms.FirstOrDefault(platform => platform.Name.Equals(platformType.ToString()));

        if (platform is null)
            return false;

        Platforms.Remove(platform);

        return true;
    }

    public bool AddGenre(GenreType genreType)
        => Genres.Add(Genre.Create(genreType));

    public bool RemoveGenre(GenreType genreType)
    {
        var genre = Genres.FirstOrDefault(genre => genre.Name.Equals(genreType.ToString()));

        if (genre is null)
            return false;

        Genres.Remove(genre);

        return true;
    }
}
