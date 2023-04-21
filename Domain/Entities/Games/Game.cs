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

    public double AverageRating => GameUsers.Average(u => u.Rating);

    public HashSet<Genre> Genres { get; private set; } = new();

    public HashSet<Company> Companies { get; private set; } = new();

    public HashSet<UserGame> GameUsers { get; private set; } = new();

    public HashSet<Platform> Platforms { get; private set; } = new();

    public Game(string name, string description)
    {
        Id = new GameId(Guid.NewGuid());
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public bool AddPlatform(PlatformType platformType)
        => Platforms.Add(new Platform(platformType));

    public bool RemovePlatform(PlatformType platformType)
    {
        var platform = Platforms.FirstOrDefault(platform => platform.Name.Equals(platformType.ToString()));

        if (platform == null)
            return false;

        Platforms.Remove(platform);

        return true;
    }

    public bool AddGenre(GenreType genreType)
        => Genres.Add(new Genre(genreType));

    public bool RemoveGenre(GenreType genreType)
    {
        var genre = Genres.FirstOrDefault(genre => genre.Name.Equals(genreType.ToString()));

        if (genre == null)
            return false;

        Genres.Remove(genre);

        return true;
    }
}
