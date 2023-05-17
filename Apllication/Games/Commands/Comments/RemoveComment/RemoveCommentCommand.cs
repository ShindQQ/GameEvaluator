using Domain.Entities.Comments;
using Domain.Entities.Users;
using MediatR;

namespace Application.Games.Commands.Comments.RemoveComment;

public record RemoveCommentCommand(CommentId Id, UserId UserId) : IRequest;
