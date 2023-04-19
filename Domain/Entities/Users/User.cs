using Domain.Entities.Companies;
using Domain.Entities.Intermidiate;
using Domain.Entities.Roles;

namespace Domain.Entities.Users;

public sealed class User
{
    public UserId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string Password { get; private set; } = string.Empty;

    public HashSet<UserGame> Games { get; private set; } = new();

    public HashSet<Role> Roles { get; private set; } = new();

    public CompanyId? CompanyId { get; private set; }

    public Company? Company { get; private set; }
}
