using Apllication.Common.Interfaces.Repositories.Base;
using Domain.Entities.Users;

namespace Apllication.Common.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, UserId>
{
    Task<User?> FindByEmailAsync(string email);

    Task<User?> FindByNameAsync(string name);

    Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expirationTime);
}
