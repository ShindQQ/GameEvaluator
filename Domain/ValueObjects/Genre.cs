using Domain.Enums;

namespace Domain.ValueObjects;

public record Genre
{
    public Genre(GenreType genreType) => Name = genreType.ToString();

    public string Name { get; init; }
}