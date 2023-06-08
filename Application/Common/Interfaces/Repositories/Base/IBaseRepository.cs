namespace Application.Common.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity, TEntityId>
    where TEntity : class
    where TEntityId : class
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> GetAsync();

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
