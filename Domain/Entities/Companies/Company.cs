using Domain.Entities.Games;
using Domain.Entities.Users;

namespace Domain.Entities.Companies;

public sealed class Company
{
    public CompanyId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public HashSet<Game> Games { get; private set; } = new();

    public HashSet<User> Workers { get; private set; } = new();

    public static Company Create(string name, string description)
    {
        var company = new Company
        {
            Id = new CompanyId(Guid.NewGuid()),
            Name = name,
            Description = description
        };

        return company;
    }

    public void AddGame(Game game)
    {
        Games.Add(game);
    }

    public void RemoveGame(Game game)
    {
        Games.Remove(game);
    }


}
