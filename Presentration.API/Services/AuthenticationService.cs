using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Apllication.Common.Models;
using Apllication.Common.Models.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presentration.API.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Presentration.API.Services;

public sealed class AuthenticationService : IAuthService
{
    private readonly IUserRepository _userRepository;

    private readonly AuthOptions _authOptions;

    public AuthenticationService(IUserRepository userRepository, IOptions<AuthOptions> authOptions)
    {
        _userRepository = userRepository;
        _authOptions = authOptions.Value;
    }

    public async Task<TokenModel?> LoginAsync(AuthModel authModel)
    {
        var user = await _userRepository.FindByEmailAsync(authModel.Email);

        if (user is not null && user.VerifyPassword(authModel.Password))
        {
            var userRoles = user.Roles;

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddHours(1).ToString())
            };

            if (user.CompanyId is not null)
                authClaims.Add(new Claim("CompanyId", user.CompanyId!.Value.ToString()));

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Name));
            }

            var token = CreateAccessToken(authClaims);
            var refreshToken = CreateRefreshToken();

            await _userRepository.UpdateRefreshTokenAsync(user, refreshToken, DateTime.Now.AddDays(1));

            return new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }

        return null;
    }

    public async Task<RefreshTokenModel?> LoginWithRefreshTokenAsync(RefreshTokenModel tokenModel)
    {
        var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);

        if (principal is null)
            return null;

        var username = principal?.Identity!.Name;
        var user = await _userRepository.FindByNameAsync(username!);

        if (user == null ||
            user.RefreshToken != tokenModel.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            return null;

        var newRefreshToken = CreateRefreshToken();

        await _userRepository.UpdateRefreshTokenAsync(user, newRefreshToken, DateTime.Now.AddDays(1));

        return new RefreshTokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(CreateAccessToken(principal!.Claims.ToList())),
            RefreshToken = newRefreshToken
        };
    }


    private JwtSecurityToken CreateAccessToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretForKey));

        return new JwtSecurityToken(claims: authClaims,
                                            expires: DateTime.Now.AddHours(1),
                                            signingCredentials: new SigningCredentials(
                                                authSigningKey, SecurityAlgorithms.HmacSha256));
    }

    private static string CreateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretForKey)),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token,
                                                   tokenValidationParameters,
                                                   out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg
            .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token!");

        return principal;
    }
}
