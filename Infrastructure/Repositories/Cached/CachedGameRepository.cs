using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories.Cached;

public sealed class CachedGameRepository : IGameRepository
{
    private readonly IGameRepository _repository;

    private readonly IMemoryCache _memoryCache;

    public CachedGameRepository(IGameRepository repository, IMemoryCache memoryCache)
    {
        _repository = repository;
        _memoryCache = memoryCache;
    }

    public async Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken = default)
        => await _memoryCache.GetOrCreateAsync(
            id,
            entity =>
            {
                entity.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                return _repository.GetByIdAsync(id, cancellationToken);
            });

    public Task<Game> AddAsync(Game entity, CancellationToken cancellationToken = default)
        => _repository.AddAsync(entity, cancellationToken);

    public Task DeleteAsync(Game entity, CancellationToken cancellationToken = default)
        => _repository.DeleteAsync(entity, cancellationToken);

    public Task<IQueryable<Game>> GetAsync()
        => _repository.GetAsync();

    public Task UpdateAsync(Game entity, CancellationToken cancellationToken = default)
        => _repository.UpdateAsync(entity, cancellationToken);
}
