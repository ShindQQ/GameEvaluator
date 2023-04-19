using Domain.Entities.Users;

namespace Domain.Entities.Roles;

public sealed class Role
{
    public RoleId Id { get; private set; } = null!;

    public RoleType Name { get; private set; }

    public List<User> Users { get; private set; } = new();
}

public enum RoleType
{
    User,
    Admin,
    Company
}
