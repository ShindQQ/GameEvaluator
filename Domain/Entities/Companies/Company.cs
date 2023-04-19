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
}
