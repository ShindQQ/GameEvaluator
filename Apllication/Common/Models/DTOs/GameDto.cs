namespace Application.Common.Models.DTOs;

public sealed class GameDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public double? AverageRating { get; init; }

    public int? UsersAmmount { get; init; }

    public List<string> Genres { get; init; } = new();

    public List<string> CompaniesNames { get; init; } = new();

    public List<string> Platforms { get; init; } = new();
}
