using Domain.Entities.Users;

namespace Apllication.Common.Interfaces;

public interface IUserService
{
    public UserId? UserId { get; }
}
