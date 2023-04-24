using Domain.Enums;

namespace Domain.ValueObjects;

public record Role
{
    public static Role Create(RoleType roleType) => new() { Name = roleType.ToString() };

    public string Name { get; init; } = string.Empty;
}
