using Domain.Entities.Companies;
using Domain.Entities.Users;
using MediatR;

namespace Application.Companies.Commands.Workers.RemoveWorker;

public record RemoveWorkerCommand : IRequest
{
    public CompanyId? CompanyId { get; init; }

    public UserId UserId { get; init; } = null!;
}
