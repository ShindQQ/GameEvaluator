using Application.Common.Requests;
using Application.Games.Commands.DeleteCommand;
using Application.Games.Commands.Genres.AddGenre;
using Application.Games.Commands.Genres.RemoveGenre;
using Application.Games.Commands.Platforms.AddPlatform;
using Application.Games.Commands.Platforms.RemovePlatform;
using Application.Games.Commands.UpdateCommand;
using Application.Games.Queries;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using Domain.Entities.Platforms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class GamesController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public GamesController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [Authorize(Roles = "Company")]
    [HttpPut("{gameId}/platforms/{platformId}")]
    public async Task<IActionResult> AddPlatformAsync(
        [FromRoute] GameId gameId,
        [FromRoute] PlatformId platformId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddPlatformToGameCommand
        {
            GameId = gameId,
            PlatformId = platformId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPut("{gameId}/companies/{companyId}/platforms/{platformId}")]
    public async Task<IActionResult> AddPlatformAsync(
        [FromRoute] GameId gameId,
        [FromRoute] CompanyId companyId,
        [FromRoute] PlatformId platformId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddPlatformToGameCommand
        {
            GameId = gameId,
            CompanyId = companyId,
            PlatformId = platformId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "Company")]
    [HttpDelete("{gameId}/platforms/{platformId}")]
    public async Task<IActionResult> RemovePlatformAsync(
        [FromRoute] GameId gameId,
        [FromRoute] PlatformId platformId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemovePlatformFromGameCommand
        {
            GameId = gameId,
            PlatformId = platformId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpDelete("{gameId}/companies/{companyId}/platforms/{platformId}")]
    public async Task<IActionResult> RemovePlatformAsync(
        [FromRoute] GameId gameId,
        [FromRoute] CompanyId companyId,
        [FromRoute] PlatformId platformId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemovePlatformFromGameCommand
        {
            GameId = gameId,
            CompanyId = companyId,
            PlatformId = platformId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "Company")]
    [HttpPut("{gameId}/genres/{genreId}")]
    public async Task<IActionResult> AddGenreAsync(
        [FromRoute] GameId gameId,
        [FromRoute] GenreId genreId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddGenreToGameCommand
        {
            GameId = gameId,
            GenreId = genreId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPut("{gameId}/companies/{companyId}/genres/{genreId}")]
    public async Task<IActionResult> AddGenreAsync(
        [FromRoute] GameId gameId,
        [FromRoute] CompanyId companyId,
        [FromRoute] GenreId genreId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddGenreToGameCommand
        {
            GameId = gameId,
            CompanyId = companyId,
            GenreId = genreId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "Company")]
    [HttpDelete("{gameId}/genres/{genreId}")]
    public async Task<IActionResult> RemoveGenreAsync(
        [FromRoute] GameId gameId,
        [FromRoute] GenreId genreId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveGenreFromGameCommand
        {
            GameId = gameId,
            GenreId = genreId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{gameId?}/{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Games")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        GameId? gameId = null)
    {
        var result = await _mediator.Send(new GameQuery
        {
            Id = gameId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{gameId}")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] GameId gameId,
        [FromBody] UpdateGameRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateGameCommand
        {
            Id = gameId,
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{gameId}")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] GameId gameId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGameCommand(gameId), cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }
}
