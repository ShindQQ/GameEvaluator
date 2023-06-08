using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Comments;

namespace Application.Common.Interfaces.Repositories;

public interface ICommentRepository : IBaseRepository<Comment, CommentId>
{
}
