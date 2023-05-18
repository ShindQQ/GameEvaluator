using Application.Common.Interfaces.Repositories;
using Application.Common.Requests;
using Application.Games.Commands.Comments.AddCommentToComment;
using Domain.Entities.Comments;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
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


        await _mediator.Send(new AddCommentToCommentCommand
        {
            ParrentCommentId = new CommentId(request.CommentId),
            GameId = new GameId(request.GameId),
            Text = request.Text,
        });

        await SendMessageAsync(request.Text);
    }

    private async Task SendMessageAsync(string message)
    {
        var connectionIds = _dictionary.Select(x => x.Key);
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
