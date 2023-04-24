using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Companies.Commands.Workers.RemoveWorker;

public sealed class RemoveWorkerCommandHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    public RemoveWorkerCommandHandler(
        ICompanyRepository companyRepository,
        IApplicationDbContext context)
    {
        _companyRepository = companyRepository;
        _context = context;
    }

    public async Task Handle(RemoveWorkerCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);

        if (company is null)
            throw new NullReferenceException(nameof(company));

        company.RemoveWorker(request.UserId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
