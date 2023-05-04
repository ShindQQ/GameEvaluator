using Domain.Entities.Companies;
using MediatR;

namespace Application.Companies.Commands.DeleteCommand;

public record DeleteCompanyCommand(CompanyId Id) : IRequest;
