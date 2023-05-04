using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Users;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, UserId>
{
}
