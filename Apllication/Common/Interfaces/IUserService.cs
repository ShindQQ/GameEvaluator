using Domain.Entities.Companies;
using Domain.Entities.Users;
using Domain.Enums;

namespace Apllication.Common.Interfaces;

public interface IUserService
{
    public UserId? UserId { get; }

    public CompanyId? CompanyId { get; }

    public RoleType RoleType { get; }
}
