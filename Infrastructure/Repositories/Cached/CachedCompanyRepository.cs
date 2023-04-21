using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories.Cached;

public sealed class CachedCompanyRepository : ICompanyRepository
{
    private readonly ICompanyRepository _repository;

    private readonly IMemoryCache _memoryCache;

    public CachedCompanyRepository(ICompanyRepository repository, IMemoryCache memoryCache)
    {
        _repository = repository;
        _memoryCache = memoryCache;
    }

    public async Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default)
        => await _memoryCache.GetOrCreateAsync(
            id,
            entity =>
            {
                entity.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                return _repository.GetByIdAsync(id, cancellationToken);
            });

    public Task<Company> AddAsync(Company entity, CancellationToken cancellationToken = default)
        => _repository.AddAsync(entity, cancellationToken);

    public Task DeleteAsync(Company entity, CancellationToken cancellationToken = default)
        => _repository.DeleteAsync(entity, cancellationToken);

    public Task<IQueryable<Company>> GetAsync()
        => _repository.GetAsync();

    public Task UpdateAsync(Company entity, CancellationToken cancellationToken = default)
        => _repository.UpdateAsync(entity, cancellationToken);
}
