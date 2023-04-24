using Apllication.Games.Commands.CreateCommand;
using Apllication.Games.Commands.DeleteCommand;
using Apllication.Games.Commands.Genres.AddGenre;
using Apllication.Games.Commands.Genres.RemoveGenre;
using Apllication.Games.Commands.Platforms.AddPlatform;
using Apllication.Games.Commands.Platforms.RemovePlatform;
using Apllication.Games.Commands.UpdateCommand;
using Apllication.Games.Queries;
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

    [HttpPost("AddPlatform")]
    public async Task<IActionResult> AddPlatformAsync([FromBody] AddPlatformCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("RemovePlatform")]
    public async Task<IActionResult> RemovePlatformAsync([FromBody] RemovePlatformCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("AddGenre")]
    public async Task<IActionResult> AddGenreAsync([FromBody] AddGenreCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("RemoveGenre")]
    public async Task<IActionResult> RemoveGenreAsync([FromBody] RemoveGenreCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("GetGames")]
    public async Task<IActionResult> GetAsync([FromBody] GameQuery request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateGameCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteGameCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }
}
