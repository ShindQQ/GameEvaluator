using Application.Common.Interfaces.Repositories.Base;
using Infrastructure.DbContexts;

namespace Application.Infrastructure;

public abstract class BaseRepository<TEntity, TEntityId>
    : IBaseRepository<TEntity, TEntityId>
       where TEntity : class
       where TEntityId : class
{
    protected readonly ApplicationDbContext Context;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual Task<IQueryable<TEntity>> GetAsync()
        => Task.FromResult(Context.Set<TEntity>().AsQueryable());

    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken)
        => await Context.Set<TEntity>().FindAsync(id, cancellationToken);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }
}
