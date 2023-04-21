using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Users;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories.Cached;

public sealed class CachedUserRepository : IUserRepository
{
    private readonly IUserRepository _repository;

    private readonly IMemoryCache _memoryCache;

    public CachedUserRepository(IUserRepository repository, IMemoryCache memoryCache)
    {
        _repository = repository;
        _memoryCache = memoryCache;
    }

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await _memoryCache.GetOrCreateAsync(
            id,
            entity =>
            {
                entity.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                return _repository.GetByIdAsync(id, cancellationToken);
            });

    public Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
        => _repository.AddAsync(entity, cancellationToken);

    public Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
        => _repository.DeleteAsync(entity, cancellationToken);

    public Task<IQueryable<User>> GetAsync()
        => _repository.GetAsync();

    public Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        => _repository.UpdateAsync(entity, cancellationToken);
}
