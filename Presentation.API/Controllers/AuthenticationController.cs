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

        return Ok(tokenModel);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
    {
        var tokenModel = await _authService.LoginWithRefreshTokenAsync(refreshTokenModel);

        return Ok(tokenModel);
    }
}
