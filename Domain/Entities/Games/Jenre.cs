namespace Domain.Entities.Games;

public record Jenre
{
    private const int MaxLength = 15;

    private Jenre(string name) => Name = name;

    public string Name { get; init; }

    public static Jenre? Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        if (name.Length >= MaxLength)
            return null;

        return new Jenre(name);
    }
}