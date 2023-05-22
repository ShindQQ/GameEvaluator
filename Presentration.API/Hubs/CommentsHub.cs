using Application.Comments.Commands.AddCommentToComment;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Requests;
using Domain.Entities.Comments;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Presentration.API.Hubs;

[Authorize]
public sealed class CommentsHub : Hub, ICommentsHub
{
    private readonly IMediator _mediator;

    private readonly IUserRepository _userRepository;

    private readonly static ConcurrentDictionary<Guid, List<string>> _dictionary = new();

    public CommentsHub(
        IMediator mediator,
        IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [HubMethodName("AddCommentToComment")]
    public async Task AddCommentToCommentAsync(AddCommentToCommentRequest request)
    {
        var userWithParentComment = await (await _userRepository.GetAsync())
            .Include(user => user.Comments.Where(comment => comment.Id.Value == request.CommentId))
            .FirstOrDefaultAsync();

        if (userWithParentComment is null || userWithParentComment.Comments.Count == 0)
            throw new NotFoundException($"Comment with id {request.CommentId} was not found!");

        await _mediator.Send(new AddCommentToCommentCommand
        {
            ParrentCommentId = new CommentId(request.CommentId),
            GameId = new GameId(request.GameId),
            Text = request.Text,
        });

        await SendMessageAsync(userWithParentComment.Id, request.Text);
    }

    private async Task SendMessageAsync(UserId userId, string message)
    {
        if (_dictionary.TryGetValue(userId.Value, out var connectionIds))
            foreach (var connection in connectionIds)
            {
                await Clients.User(connection.ToString()).SendAsync("ReceiveComment", message);
            }
    }

    public override async Task OnConnectedAsync()
    {
        var user = await _userRepository.GetByIdAsync(new UserId(Guid.Parse(Context.UserIdentifier!)));

        _dictionary.AddOrUpdate(user!.Id.Value, new List<string> { Context.ConnectionId }, (key, connectionIds) =>
        {
            if (!connectionIds.Any(connectionId => connectionId.Equals(Context.ConnectionId)))
            {
                connectionIds.Add(Context.ConnectionId);

                return connectionIds;
            }

            return connectionIds;
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(Guid.Parse(Context.UserIdentifier!)));

        if (_dictionary.TryGetValue(user!.Id.Value, out var connectionIds))
        {
            connectionIds.Remove(Context.ConnectionId);
            _dictionary.TryUpdate(user!.Id.Value, connectionIds, connectionIds);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
