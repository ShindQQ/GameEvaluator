using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Comments.Commands.UpdateComment;

public sealed class UpdateCommentCommandhandler : IRequestHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    private readonly IUserRepository _userRepository;

    private readonly IUserService _userService;

    public UpdateCommentCommandhandler(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IUserService userService)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId is null ? _userService.UserId : request.UserId;

        var user = await (await _userRepository.GetAsync())
            .Include(user => user.Comments.Where(comment => comment.Id == request.Id))
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {userId} was not found!");

        var comment = user.Comments.FirstOrDefault()
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Comment with id {request.Id} was not found!");

        comment.Update(request.Text);

        await _commentRepository.UpdateAsync(comment, cancellationToken);
    }
}
