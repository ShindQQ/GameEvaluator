using Domain.Enums;

namespace Domain.ValueObjects;

public record Role
{
    public Role(RoleType roleType) => Name = roleType.ToString();

    public string Name { get; init; }
}
