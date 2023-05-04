using Application.Common.Models;
using Application.Common.Models.Tokens;

namespace Application.Common.Interfaces;

public interface IAuthService
{
    Task<TokenModel?> LoginAsync(AuthModel authModel);

    Task<RefreshTokenModel?> LoginWithRefreshTokenAsync(RefreshTokenModel tokenModel);
}
