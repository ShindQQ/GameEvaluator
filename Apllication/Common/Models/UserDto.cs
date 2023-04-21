using Apllication.Common.Mappings;
using Domain.Entities.Users;
using Domain.Enums;

namespace Apllication.Common.Models;

public sealed class UserDto : IMapFrom<User>
{
    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public List<GameDto> Games { get; init; } = new();

    public List<RoleType> Roles { get; init; } = new();

    public Guid? CompanyId { get; init; }

    public CompanyDto? Company { get; init; }
}
