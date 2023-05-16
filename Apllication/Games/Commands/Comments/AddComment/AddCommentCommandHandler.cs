using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Comments;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Games.Commands.Comments.AddComment;

public sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentId>
{
    private readonly IGameRepository _gameRepository;

    private readonly IUserRepository _userRepository;

    private readonly ICommentRepository _commentRepository;

    private readonly IApplicationDbContext _dbContext;

    public AddCommentCommandHandler(IGameRepository gameRepository,
        ICommentRepository commentRepository,
        IApplicationDbContext dbContext,
        IUserRepository userRepository)
    {
        _gameRepository = gameRepository;
        _commentRepository = commentRepository;
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    public async Task<CommentId> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken)
            ?? throw new NotFoundException(nameof(Game), request.GameId);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var comment = Comment.Create(request.Text, game, user);

        await _commentRepository.AddAsync(comment, cancellationToken);

        game.AddComment(comment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}
