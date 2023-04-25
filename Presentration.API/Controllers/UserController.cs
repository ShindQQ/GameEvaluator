﻿using Apllication.Common.Requests;
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
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public UserController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request);

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
        });

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
        });

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{userId?}/{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Users")]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        UserId? userId = null)
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
        });

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] UserId userId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(userId));

        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }
}
