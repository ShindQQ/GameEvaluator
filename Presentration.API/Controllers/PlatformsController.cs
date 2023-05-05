using Application.Common.Requests;
using Application.Platforms.Commands.CreateCommand;
using Application.Platforms.Commands.DeleteCommand;
using Application.Platforms.Commands.UpdateCommand;
using Application.Platforms.Queries;
using Domain.Entities.Platforms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "SuperAdmin, Company, Admin")]
public sealed class PlatformsController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public PlatformsController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePlatformCommand request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        await _cache.EvictByTagAsync("platforms", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Platforms")]
    [HttpGet("{platformId?}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        PlatformId? platformId = null)
    {
        var result = await _mediator.Send(new PlatformQuery
        {
            Id = platformId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{platformId}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] PlatformId genreId,
        [FromBody] UpdatePlatformRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdatePlatformCommand
        {
            Id = genreId,
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        await _cache.EvictByTagAsync("platforms", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{platformId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] PlatformId platformId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePlatformCommand(platformId), cancellationToken);

        await _cache.EvictByTagAsync("platforms", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }
}
