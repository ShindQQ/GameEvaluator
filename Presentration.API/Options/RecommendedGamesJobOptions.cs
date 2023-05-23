namespace Presentration.API.Options;

public record RecommendedGamesJobOptions
{
    public int TimerValue { get; init; }

    public int AmountOfGames { get; init; }
}
