using Domain.Entities.Comments;
using MediatR;

namespace Application.Games.Commands.Comments.RemoveComment;

public record RemoveCommentCommand(CommentId Id) : IRequest;
