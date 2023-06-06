using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Comments.Commands.RemoveComment;

public sealed class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    private readonly IUserRepository _userRepository;

    public RemoveCommentCommandHandler(
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await (await _userRepository.GetAsync())
            .Include(user => user.Comments.Where(comment => comment.Id == request.Id))
            .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {request.UserId} was not found!");

        var comment = user.Comments.FirstOrDefault()
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Comment with id {request.Id} was not found!");

        await _commentRepository.DeleteAsync(comment, cancellationToken);
    }
}
