using Application.Comments.Commands.AddComment;
using Application.Comments.Commands.AddCommentToComment;
using Application.Comments.Commands.UpdateComment;
using Application.Common.Requests;
using Domain.Entities.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public CommentsController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public async Task<IActionResult> AddCommentAsync(
    [FromBody] AddCommentCommand command,
    CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("users", cancellationToken);

        return Ok(id);
    }

    [HttpPost("{commentId}")]
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public async Task<IActionResult> AddCommentToCommentAsync(
        [FromRoute] CommentId commentId,
        [FromBody] CommentRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new AddCommentToCommentCommand
        {
            ParrentCommentId = commentId,
            UserId = request.UserId,
            Text = request.Text,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("users", cancellationToken);

        return Ok(id);
    }

    [HttpPatch("{commentId}")]
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public async Task<IActionResult> UpdateCommentAsync(
        [FromRoute] CommentId commentId,
        [FromBody] CommentRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateCommentCommand
        {
            Id = commentId,
            UserId = request.UserId,
            Text = request.Text,
        }, cancellationToken);

        await _cache.EvictByTagAsync("games", cancellationToken);
        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("users", cancellationToken);

        return NoContent();
    }
}
