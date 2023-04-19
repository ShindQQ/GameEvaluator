using Domain.Entities.Companies;
using Domain.Entities.Intermidiate;
using Domain.Entities.Jenres;
using Domain.Entities.Platforms;

namespace Domain.Entities.Games;

public sealed class Game
{
    public GameId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public double AverageRating => GameUsers.Average(u => u.Rating);

    public HashSet<Jenre> Jenres { get; private set; } = new();

    public HashSet<Company> Companies { get; private set; } = new();

    public HashSet<UserGame> GameUsers { get; private set; } = new();

    public HashSet<Platform> Platforms { get; private set; } = new();
}
