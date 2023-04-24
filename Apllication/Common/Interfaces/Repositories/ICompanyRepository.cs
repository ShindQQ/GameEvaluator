using Apllication.Common.Interfaces.Repositories.Base;
using Domain.Entities.Companies;

namespace Apllication.Common.Interfaces.Repositories;

public interface ICompanyRepository : IBaseRepository<Company, CompanyId>
{
}
