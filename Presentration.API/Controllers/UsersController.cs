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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "SuperAdmin, Admin")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public UsersController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);

        return Ok(result);
    }

    [HttpPut("{userId}/roles/{roleType}")]
    public async Task<IActionResult> AddRoleAsync(
        [FromRoute] UserId userId,
        [FromRoute] RoleType roleType,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddRoleCommand
        {
            UserId = userId,
            RoleType = roleType,
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId}/roles/{roleType}")]
    public async Task<IActionResult> RemoveRoleAsync(
        [FromRoute] UserId userId,
        [FromRoute] RoleType roleType,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveRoleCommand
        {
            UserId = userId,
            RoleType = roleType,
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{userId?}/{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Users")]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        UserId? userId = null)
    {
        var result = await _mediator.Send(new UserQuery
        {
            Id = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{userId}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] UserId userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateUserCommand
        {
            Id = userId,
            Name = request.Name,
            Email = request.Email,
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] UserId userId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }
}
