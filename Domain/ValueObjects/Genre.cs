using Domain.Enums;

namespace Domain.ValueObjects;

public record Genre
{
    private Genre(string name) => Name = name;

    public string Name { get; init; }

    public static Genre? Create(GenreType genreType)
    {
        return new Genre(genreType.ToString());
    }
}