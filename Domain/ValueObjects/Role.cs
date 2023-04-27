using Domain.Enums;

namespace Domain.ValueObjects;

public record Role
{
    public static Role Create(RoleType roleType) => new() { Name = roleType.ToString() };

    public static Role Create(string roleType) => new() { Name = roleType };

    public string Name { get; init; } = string.Empty;
}
