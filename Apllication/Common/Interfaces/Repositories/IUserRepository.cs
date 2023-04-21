using Domain.Entities.Users;

namespace Apllication.Common.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, UserId>
{
}
