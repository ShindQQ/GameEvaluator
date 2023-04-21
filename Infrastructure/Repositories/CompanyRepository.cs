using Apllication.Common.Interfaces.Repositories;
using Apllication.Infrastructure;
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
            .Include(company => company.Games)
            .Include(company => company.Workers)
            .AsQueryable());

    public override async Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken)
        => await Context.Companies
            .Include(company => company.Games)
            .Include(company => company.Workers)
            .FirstOrDefaultAsync(company => company.Id == id, cancellationToken);
}
