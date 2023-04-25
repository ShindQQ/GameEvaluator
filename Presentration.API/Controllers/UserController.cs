using Apllication.Common.Requests;
using Apllication.Users.Commands.CreateCommand;
using Apllication.Users.Commands.DeleteCommand;
using Apllication.Users.Commands.Roles.AddRole;
using Apllication.Users.Commands.Roles.RemoveRole;
using Apllication.Users.Commands.UpdateCommand;
using Aplliction.Users.Queries;
using Domain.Entities.Users;
using Domain.Enums;
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

    [HttpPut("{userId}/roles/{roleType}")]
    public async Task<IActionResult> AddRoleAsync([FromRoute] UserId userId, [FromRoute] RoleType roleType)
    {
        await _mediator.Send(new AddRoleCommand
        {
            UserId = userId,
            RoleType = roleType,
        });

        return NoContent();
    }

    [HttpDelete("{userId}/roles/{roleType}")]
    public async Task<IActionResult> RemoveRoleAsync([FromRoute] UserId userId, [FromRoute] RoleType roleType)
    {
        await _mediator.Send(new RemoveRoleCommand
        {
            UserId = userId,
            RoleType = roleType,
        });

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{userId?}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, UserId? userId = null)
    {
        var result = await _mediator.Send(new UserQuery
        {
            Id = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        return Ok(result);
    }

    [HttpPatch("{userId}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] UserId userId, [FromBody] UpdateUserRequest request)
    {
        await _mediator.Send(new UpdateUserCommand
        {
            Id = userId,
            Name = request.Name,
            Email = request.Email,
        });

        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] UserId userId)
    {
        await _mediator.Send(new DeleteUserCommand(userId));

        return NoContent();
    }
}
