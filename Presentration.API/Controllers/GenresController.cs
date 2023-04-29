using Apllication.Common.Requests;
using Apllication.Genres.Commands.CreateCommand;
using Apllication.Genres.Commands.DeleteCommand;
using Apllication.Genres.Commands.UpdateCommand;
using Apllication.Genres.Queries;
using Domain.Entities.Genres;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "SuperAdmin, Admin")]
public sealed class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public GenresController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateGenreCommand request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        await _cache.EvictByTagAsync("genres", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return Ok(response);
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Genres")]
    [HttpGet("{genreId?}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        GenreId? genreId = null)
    {
        var result = await _mediator.Send(new GenreQuery
        {
            Id = genreId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{genreId}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] GenreId genreId,
        [FromBody] UpdateGenreRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateGenreCommand
        {
            Id = genreId,
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        await _cache.EvictByTagAsync("genres", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{genreId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] GenreId genreId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGenreCommand(genreId), cancellationToken);

        await _cache.EvictByTagAsync("genres", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }
}
