using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Companies.Commands.Workers.AddWorker;

public sealed class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    public AddWorkerCommandHandler(
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        IApplicationDbContext context)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);

        if (company is null)
            throw new NullReferenceException(nameof(company));

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new NullReferenceException(nameof(user));

        company.AddWorker(user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
