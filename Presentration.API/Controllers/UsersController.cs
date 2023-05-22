﻿using Aplliction.Users.Queries;
using Application.Comments.Commands.RemoveComment;
using Application.Comments.Queries.UserComments;
using Application.Common.Requests;
using Application.Users.Commands.Bans.Ban;
using Application.Users.Commands.Bans.Unban;
using Application.Users.Commands.CreateCommand;
using Application.Users.Commands.DeleteCommand;
using Application.Users.Commands.Games.AddGame;
using Application.Users.Commands.Games.SetFavorite;
using Application.Users.Commands.Games.SetRating;
using Application.Users.Commands.Roles.AddRole;
using Application.Users.Commands.Roles.RemoveRole;
using Application.Users.Commands.UpdateCommand;
using Application.Users.Queries;
using Domain.Entities.Comments;
using Domain.Entities.Games;
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

    [OutputCache(PolicyName = "Users")]
    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{userId?}/{pageNumber}/{pageSize}")]
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

    [HttpGet("/recomended/{userId}/{ammountOfGames}")]
    public async Task<IActionResult> GetRecomendedGamesAsync(
        UserId userId,
        int ammountOfGames,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RecomendedGamesQuery
        {
            UserId = userId,
            AmountOfGames = ammountOfGames,
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

    [HttpPut("/{userId}/games/{gameId}")]
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public async Task<IActionResult> AddGameAsync(
        [FromRoute] UserId userId,
        [FromRoute] GameId gameId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddGameToUserCommand
        {
            UserId = userId,
            GameId = gameId
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpPut("/{userId}/games/{gameId}/ratings/{rating}")]
    public async Task<IActionResult> SetRatingAsync(
        [FromRoute] UserId userId,
        [FromRoute] GameId gameId,
        [FromRoute] int rating,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new SetRatingCommand
        {
            UserId = userId,
            GameId = gameId,
            Rating = rating
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpPut("/{userId}/games/{gameId}/favorites")]
    public async Task<IActionResult> SetFavoriteAsync(
        [FromRoute] UserId userId,
        [FromRoute] GameId gameId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new SetGameAsFavoriteCommand
        {
            UserId = userId,
            GameId = gameId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpPut("/ban")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> BanUserAsync(
        BanCommand banCommand,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(banCommand, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpPut("/{userId}/unban")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> UnbanUserAsync(
        [FromRoute] UserId userId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UnbanCommand
        {
            UserId = userId,
        }, cancellationToken);

        await _cache.EvictByTagAsync("users", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId}/Comments/{commentId}")]
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public async Task<IActionResult> RemoveCommentAsync(
        [FromRoute] UserId userId,
        [FromRoute] CommentId commentId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveCommentCommand(commentId, userId), cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }

    [HttpGet("{userId}/Comments/{pageNumber}/{pageSize}")]
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public async Task<IActionResult> GetCommentsAsync(
        [FromRoute] UserId userId,
        [FromRoute] int pageNumber,
        [FromRoute] int pageSize,
        CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(new UserCommentsQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("users", cancellationToken);

        return Ok(comments);
    }
}
