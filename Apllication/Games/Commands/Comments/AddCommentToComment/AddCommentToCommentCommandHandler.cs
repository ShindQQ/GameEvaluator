﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Comments;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Games.Commands.Comments.AddCommentToComment;

public sealed class AddCommentToCommentCommandHandler : IRequestHandler<AddCommentToCommentCommand, CommentId>
{
    private readonly IGameRepository _gameRepository;

    private readonly IUserRepository _userRepository;

    private readonly ICommentRepository _commentRepository;

    private readonly IApplicationDbContext _dbContext;

    private readonly IUserService _userService;

    public AddCommentToCommentCommandHandler(IGameRepository gameRepository,
        ICommentRepository commentRepository,
        IApplicationDbContext dbContext,
        IUserRepository userRepository,
        IUserService userService)
    {
        _gameRepository = gameRepository;
        _commentRepository = commentRepository;
        _dbContext = dbContext;
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<CommentId> Handle(AddCommentToCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId is null ? _userService.UserId : request.UserId;

        var comment = await _commentRepository.GetByIdAsync(request.ParrentCommentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Comment), request.ParrentCommentId);

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken)
            ?? throw new NotFoundException(nameof(Game), request.GameId);

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), userId);

        var child = comment.CreateChild(request.Text, user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return child.Id;
    }
}