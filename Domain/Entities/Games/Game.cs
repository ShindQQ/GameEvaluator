using Domain.Entities.Companies;
using Domain.Entities.Intermidiate;
using Domain.Enums;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Games;

public sealed class Game
{
    [NotMapped]
    private const int NameLength = 20;

    [NotMapped]
    private const int DescriptionLength = 200;

    public GameId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public double AverageRating => GameUsers.Average(u => u.Rating);

    public HashSet<Genre> Genres { get; private set; } = new();

    public HashSet<Company> Companies { get; private set; } = new();

    public HashSet<UserGame> GameUsers { get; private set; } = new();

    public HashSet<Platform> Platforms { get; private set; } = new();

    public static Game? Create(string name, string description,
        HashSet<GenreType> genreTypes, HashSet<PlatformType> platformTypes)
    {
        if (string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(description))
            return null;

        if (name.Length > NameLength ||
            description.Length > DescriptionLength)
            return null;

        var game = new Game
        {
            Name = name,
            Description = description,
        };

        foreach (var genre in genreTypes)
        {
            game.Genres.Add(Genre.Create(genre));
        }

        foreach (var platform in platformTypes)
        {
            game.Platforms.Add(Platform.Create(platform));
        }

        return game;
    }
}
