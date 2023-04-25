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

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGameCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPut("{gameId}/platforms/{platformType}")]
    public async Task<IActionResult> AddPlatformAsync([FromRoute] GameId gameId, [FromRoute] PlatformType platformType)
    {
        await _mediator.Send(new AddPlatformCommand
        {
            GameId = gameId,
            PlatformType = platformType,
        });

        return NoContent();
    }

    [HttpDelete("{gameId}/platforms/{platformType}")]
    public async Task<IActionResult> RemovePlatformAsync([FromRoute] GameId gameId, [FromRoute] PlatformType platformType)
    {
        await _mediator.Send(new RemovePlatformCommand
        {
            GameId = gameId,
            PlatformType = platformType,
        });

        return NoContent();
    }

    [HttpPut("{gameId}/genres/{genreType}")]
    public async Task<IActionResult> AddGenreAsync([FromRoute] GameId gameId, [FromRoute] GenreType genreType)
    {
        await _mediator.Send(new AddGenreCommand
        {
            GameId = gameId,
            GenreType = genreType,
        });

        return NoContent();
    }

    [HttpDelete("{gameId}/genres/{genreType}")]
    public async Task<IActionResult> RemoveGenreAsync([FromRoute] GameId gameId, [FromRoute] GenreType genreType)
    {
        await _mediator.Send(new RemoveGenreCommand
        {
            GameId = gameId,
            GenreType = genreType,
        });

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{gameId?}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, GameId? gameId = null)
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
    public async Task<IActionResult> UpdateAsync([FromRoute] GameId gameId, [FromBody] UpdateGameRequest request)
    {
        await _mediator.Send(new UpdateGameCommand
        {
            Id = gameId,
            Name = request.Name,
            Description = request.Description,
        });

        return NoContent();
    }

    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] GameId gameId)
    {
        await _mediator.Send(new DeleteGameCommand(gameId));

        return NoContent();
    }
}
