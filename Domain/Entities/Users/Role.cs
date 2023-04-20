namespace Domain.Entities.Users;

public record Role
{
    private Role(string name) => Name = name;

    public string Name { get; init; }

    public static Role? Create(RoleType roleType)
    {
        return new Role(roleType.ToString());
    }
}

public enum RoleType
{
    User,
    Admin,
    Company
}
