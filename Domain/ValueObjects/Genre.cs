using Domain.Enums;

namespace Domain.ValueObjects;

public record Genre
{
    public static Genre Create(GenreType genreType) => new() { Name = genreType.ToString() };

    public string Name { get; init; }
}