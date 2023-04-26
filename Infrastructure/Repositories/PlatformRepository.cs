using Apllication.Common.Interface;
using Apllication.Infrastructure;
using Domain.Entities.Platforms;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class PlatformRepository
    : BaseRepository<Platform, PlatformId>,
    IPlatformRepository
{
    public PlatformRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<Platform>> GetAsync()
        => Task.FromResult(Context.Platforms
            .AsQueryable());

    public override async Task<Platform?> GetByIdAsync(PlatformId id, CancellationToken cancellationToken)
        => await Context.Platforms
            .FirstOrDefaultAsync(platform => platform.Id == id, cancellationToken);
}
