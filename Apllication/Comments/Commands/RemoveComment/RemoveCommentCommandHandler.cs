using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Comments;
using Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var comment = user.Comments.FirstOrDefault()
            ?? throw new NotFoundException(nameof(Comment), request.Id);

        await _commentRepository.DeleteAsync(comment, cancellationToken);
    }
}
