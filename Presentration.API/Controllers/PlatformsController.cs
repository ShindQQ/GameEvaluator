using Apllication.Common.Requests;
using Apllication.Platforms.Commands.CreateCommand;
using Apllication.Platforms.Commands.DeleteCommand;
using Apllication.Platforms.Commands.UpdateCommand;
using Apllication.Platforms.Queries;
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
    [FromBody] CreatePaltformCommand request,
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        await _cache.EvictByTagAsync("platforms", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return Ok(response);
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{platformId?}/{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Platforms")]
    [AllowAnonymous]
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
