using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Comments;
using MediatR;
using System.Net;

namespace Application.Comments.Commands.AddCommentToComment;

public sealed class AddCommentToCommentCommandHandler : IRequestHandler<AddCommentToCommentCommand, CommentId>
{
    private readonly IUserRepository _userRepository;

    private readonly ICommentRepository _commentRepository;

    private readonly IApplicationDbContext _dbContext;

    private readonly IUserService _userService;

    public AddCommentToCommentCommandHandler(
        ICommentRepository commentRepository,
        IApplicationDbContext dbContext,
        IUserRepository userRepository,
        IUserService userService)
    {
        _commentRepository = commentRepository;
        _dbContext = dbContext;
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<CommentId> Handle(AddCommentToCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId is null ? _userService.UserId : request.UserId;

        var comment = await _commentRepository.GetByIdAsync(request.ParrentCommentId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Comment with id {request.ParrentCommentId} was not found!");

        var user = await _userRepository.GetByIdAsync(userId!, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"User with id {userId} was not found!");

        var child = comment.CreateChild(request.Text, user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return child.Id;
    }
}
