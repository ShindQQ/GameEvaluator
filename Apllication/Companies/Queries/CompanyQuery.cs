using Domain.Entities.Companies;
using MediatR;

namespace Apllication.Companies.Queries;

public record CompanyQuery(CompanyId Id) : IRequest<List<Company>>;
