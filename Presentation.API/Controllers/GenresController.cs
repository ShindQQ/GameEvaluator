using Application.Common.Requests;
using Application.Genres.Commands.CreateCommand;
using Application.Genres.Commands.DeleteCommand;
using Application.Genres.Commands.UpdateCommand;
using Application.Genres.Queries;
using Domain.Entities.Genres;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Presentation.API.Controllers;

namespace Presentration.API.Controllers;

[Authorize(Roles = "SuperAdmin, Admin")]
public sealed class GenresController : BaseController
{
    public GenresController(IMediator mediator, IOutputCacheStore cache) : base(mediator, cache)
    {
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
