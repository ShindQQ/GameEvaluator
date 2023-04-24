using Apllication.Common.Interfaces;
using Domain.Entities.Users;

namespace Presentration.API.Services;

public sealed class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserId? UserId => new UserId(Guid.NewGuid());
}
