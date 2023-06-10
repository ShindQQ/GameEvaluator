using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Tokens;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Resources;
using Presentration.API.Services;
using System.Diagnostics;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    private const string SourceName = "GameEvaluator";

    private readonly ActivitySource _source;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
        _source = new ActivitySource(SourceName);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(AuthModel authModel)
    {
        using var activity = _source.StartActivity(SourceName, ActivityKind.Internal)!;

        var tokenModel = await _authService.LoginAsync(authModel);

        GameEvaluatorMetricsService.RequestCounter.Add(1,
            new("Action", nameof(LoginAsync)),
            new("Controller", nameof(AuthenticationController)));

        activity.AddEvent(new ActivityEvent("Generate AuthToken", tags: new ActivityTagsCollection(new[] 
        { 
            KeyValuePair.Create<string, object?>("Token", tokenModel.AccessToken) 
        })));

        activity.SetTag("otel.status_code", "OK");
        activity.SetTag("otel.status_description", "Token was given successfully");

        return Ok(tokenModel);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
    {
        var tokenModel = await _authService.LoginWithRefreshTokenAsync(refreshTokenModel);

        return Ok(tokenModel);
    }
}
