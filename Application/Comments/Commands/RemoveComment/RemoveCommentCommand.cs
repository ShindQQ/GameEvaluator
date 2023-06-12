using Domain.Entities.Comments;
using Domain.Entities.Users;
using MediatR;

namespace Application.Comments.Commands.RemoveComment;

public record RemoveCommentCommand(CommentId Id, UserId UserId) : IRequest;
