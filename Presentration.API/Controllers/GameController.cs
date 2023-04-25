using Apllication.Common.Requests;
using Apllication.Games.Commands.CreateCommand;
using Apllication.Games.Commands.DeleteCommand;
using Apllication.Games.Commands.Genres.AddGenre;
using Apllication.Games.Commands.Genres.RemoveGenre;
using Apllication.Games.Commands.Platforms.AddPlatform;
using Apllication.Games.Commands.Platforms.RemovePlatform;
using Apllication.Games.Commands.UpdateCommand;
using Apllication.Games.Queries;
using Domain.Entities.Games;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public GameController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGameCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPut("{gameId}/platforms/{platformType}")]
    public async Task<IActionResult> AddPlatformAsync(
        [FromRoute] GameId gameId,
        [FromRoute] PlatformType platformType,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddPlatformCommand
        {
            GameId = gameId,
            PlatformType = platformType,
        });

        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{gameId}/platforms/{platformType}")]
    public async Task<IActionResult> RemovePlatformAsync(
        [FromRoute] GameId gameId,
        [FromRoute] PlatformType platformType,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemovePlatformCommand
        {
            GameId = gameId,
            PlatformType = platformType,
        });

        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpPut("{gameId}/genres/{genreType}")]
    public async Task<IActionResult> AddGenreAsync(
        [FromRoute] GameId gameId,
        [FromRoute] GenreType genreType,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddGenreCommand
        {
            GameId = gameId,
            GenreType = genreType,
        });

        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{gameId}/genres/{genreType}")]
    public async Task<IActionResult> RemoveGenreAsync(
        [FromRoute] GameId gameId,
        [FromRoute] GenreType genreType,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveGenreCommand
        {
            GameId = gameId,
            GenreType = genreType,
        });

        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{gameId?}/{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Games")]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        GameId? gameId = null)
    {
        var result = await _mediator.Send(new GameQuery
        {
            Id = gameId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        return Ok(result);
    }

    [HttpPatch("{gameId}")]
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
        });

        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] GameId gameId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGameCommand(gameId));

        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }
}
