using Application.Common.Exceptions;
using Application.Common.Interfaces;
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
            ?? throw new NotFoundException(nameof(User), userId!);

        var comment = user.Comments.FirstOrDefault()
            ?? throw new NotFoundException(nameof(Comment), userId!);

        comment.Update(request.Text);

        await _commentRepository.UpdateAsync(comment, cancellationToken);
    }
}
