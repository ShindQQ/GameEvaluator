using Domain.Entities.Companies;
using Domain.Entities.Users;
using MediatR;

namespace Apllication.Companies.Commands.Workers.AddWorker;

public record AddWorkerCommand : IRequest
{
    public CompanyId CompanyId { get; init; } = null!;

    public UserId UserId { get; init; } = null!;
}
