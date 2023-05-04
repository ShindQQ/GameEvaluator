using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Companies.Commands.Workers.AddWorker;

public sealed class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public AddWorkerCommandHandler(
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        IApplicationDbContext context,
        IUserService userService)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _context = context;
        _userService = userService;
    }

    public async Task Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), companyId!);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.UserId);

        company.AddWorker(user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
