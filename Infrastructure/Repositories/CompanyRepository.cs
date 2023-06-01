using Application.Common.Interfaces.Repositories;
using Application.Infrastructure;
using Domain.Entities.Companies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class CompanyRepository
    : BaseRepository<Company, CompanyId>,
    ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<Company>> GetAsync()
        => Task.FromResult(Context.Companies
            .AsQueryable());

    public override async Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken)
        => await Context.Companies
            .Include(company => company.Games)
                .ThenInclude(game => game.Genres)
            .Include(company => company.Games)
                .ThenInclude(game => game.Platforms)
            .Include(company => company.Workers)
            .FirstOrDefaultAsync(company => company.Id == id, cancellationToken);
}
