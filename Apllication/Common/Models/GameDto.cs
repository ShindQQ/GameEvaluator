using Domain.Enums;

namespace Apllication.Common.Models;

public sealed class GameDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public double? AverageRating { get; init; }

    public int? UsersAmmount { get; init; }

    public List<GenreType> Genres { get; init; } = new();

    public List<CompanyDto> Companies { get; init; } = new();

    public List<PlatformType> Platforms { get; init; } = new();
}
