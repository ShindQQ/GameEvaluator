using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Companies;

namespace Application.Common.Interfaces.Repositories;

public interface ICompanyRepository : IBaseRepository<Company, CompanyId>
{
}
