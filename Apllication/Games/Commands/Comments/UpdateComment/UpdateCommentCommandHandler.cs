using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Comments;
using Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Games.Commands.Comments.UpdateComment;

public sealed class UpdateCommentCommandhandler : IRequestHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    private readonly IUserRepository _userRepository;

    public UpdateCommentCommandhandler(
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await (await _userRepository.GetAsync())
            .Include(user => user.Comments.Where(comment => comment.Id == request.Id))
            .FirstOrDefaultAsync(user => user.Id == request.UserId)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var comment = user.Comments.FirstOrDefault()
            ?? throw new NotFoundException(nameof(Comment), request.Id);

        comment.Update(request.Text);

        await _commentRepository.UpdateAsync(comment);
    }
}
