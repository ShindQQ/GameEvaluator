using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(AuthModel authModel)
    {
        var tokenModel = await _authService.LoginAsync(authModel);

        if (tokenModel is null)
            return Unauthorized($"User with email {authModel.Email} wasn`t found!");
        if (tokenModel.IsBanned)
            return Unauthorized($"User with email {authModel.Email} is banned!");
        if (tokenModel.PasswordIncorrect)
            return Unauthorized($"User with email {authModel.Email} typed wrong password!");

        return Ok(tokenModel);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
    {
        var tokenModel = await _authService.LoginWithRefreshTokenAsync(refreshTokenModel);

        if (tokenModel is null)
            return BadRequest("Invalid access or refresh token!");

        return Ok(tokenModel);
    }
}
