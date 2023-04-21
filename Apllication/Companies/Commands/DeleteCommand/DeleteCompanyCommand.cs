using Domain.Entities.Companies;
using MediatR;

namespace Apllication.Companies.Commands.DeleteCommand;

public record DeleteCompanyCommand(CompanyId Id) : IRequest;
