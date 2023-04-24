using Apllication.Users.Commands.CreateCommand;
using Apllication.Users.Commands.DeleteCommand;
using Apllication.Users.Commands.Roles.AddRole;
using Apllication.Users.Commands.Roles.RemoveRole;
using Apllication.Users.Commands.UpdateCommand;
using Aplliction.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPost("AddRole")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("RemoveRole")]
    public async Task<IActionResult> RemoveRoleAsync([FromBody] RemoveRoleCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("GetUsers")]
    public async Task<IActionResult> GetAsync([FromBody] UserQuery request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteUserCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }
}
