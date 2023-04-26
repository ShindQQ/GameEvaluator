using Apllication.Common.Models;
using Apllication.Common.Models.Tokens;

namespace Apllication.Common.Interfaces;

public interface IAuthService
{
    Task<TokenModel?> LoginAsync(AuthModel authModel);

    Task<RefreshTokenModel?> LoginWithRefreshTokenAsync(RefreshTokenModel tokenModel);
}
