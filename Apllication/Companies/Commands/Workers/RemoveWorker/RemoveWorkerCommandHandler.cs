using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Companies.Commands.Workers.RemoveWorker;

public sealed class RemoveWorkerCommandHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public RemoveWorkerCommandHandler(
        ICompanyRepository companyRepository,
        IApplicationDbContext context,
        IUserService userService)
    {
        _companyRepository = companyRepository;
        _context = context;
        _userService = userService;
    }

    public async Task Handle(RemoveWorkerCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), companyId!);

        company.RemoveWorker(request.UserId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
