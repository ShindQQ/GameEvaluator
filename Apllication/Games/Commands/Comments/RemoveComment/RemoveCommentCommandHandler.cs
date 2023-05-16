using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Comments;
using MediatR;

namespace Application.Games.Commands.Comments.RemoveComment;

internal class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    public RemoveCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Comment), request.Id);

        await _commentRepository.DeleteAsync(comment, cancellationToken);
    }
}
