using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Common.Models;
using Application.Common.Models.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presentration.API.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        var user = await (await _userRepository.GetAsync())
            .Include(user => user.Company)
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Email.Equals(authModel.Email));

        if (user is not null)
        {
            if (!user.VerifyPassword(authModel.Password))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "User entered wrong password!");
            if (user.BanState is not null)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "User is banned!");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddHours(1).ToString()),
                new Claim(ClaimTypes.Role, string.Join(',', user.Roles.Select(role => role.Name))),
            };

            if (user.CompanyId is not null)
                authClaims.Add(new Claim("CompanyId", user.CompanyId!.Value.ToString()));

            foreach (var userRole in user.Roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Name));
            }

            var token = CreateAccessToken(authClaims);
            var refreshToken = CreateRefreshToken();

            user.SetRefreshToken(refreshToken, DateTime.Now.AddDays(1));
            await _userRepository.UpdateAsync(user);

            return new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                Roles = user.Roles.Select(role => role.Name).ToList(),
            };
        }

        throw new StatusCodeException(HttpStatusCode.BadRequest, "User was not found!");
    }

    public async Task<RefreshTokenModel?> LoginWithRefreshTokenAsync(RefreshTokenModel tokenModel)
    {
        var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken) 
            ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Wrong refresh token!");

        var username = principal?.Identity!.Name;
        var user = await (await _userRepository.GetAsync())
            .FirstOrDefaultAsync(user => user.Name.Equals(username)) 
            ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "User was not found!");

        if (user.RefreshToken != tokenModel.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new StatusCodeException(HttpStatusCode.BadRequest, "Refresh token has expired!");

        var newRefreshToken = CreateRefreshToken();

        user.SetRefreshToken(newRefreshToken, DateTime.Now.AddDays(1));
        await _userRepository.UpdateAsync(user);

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
