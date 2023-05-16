using Application.Common.Interfaces.Repositories;
using Application.Infrastructure;
using Domain.Entities.Comments;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class CommentRepository
    : BaseRepository<Comment, CommentId>,
    ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<Comment>> GetAsync()
        => Task.FromResult(Context.Comments
            .AsQueryable());

    public override async Task<Comment?> GetByIdAsync(CommentId id, CancellationToken cancellationToken)
        => await Context.Comments
            .Include(comment => comment.Game)
            .Include(comment => comment.ChildrenComments)
            .FirstOrDefaultAsync(comment => comment.Id == id, cancellationToken);
}
