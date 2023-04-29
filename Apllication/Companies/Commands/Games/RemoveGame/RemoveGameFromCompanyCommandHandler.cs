using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Apllication.Companies.Commands.Games.RemoveGame;

public sealed class RemoveGameFromCompanyCommandHandler : IRequestHandler<RemoveGameFromCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public RemoveGameFromCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IApplicationDbContext context,
        IUserService userService)
    {
        _companyRepository = companyRepository;
        _context = context;
        _userService = userService;
    }

    public async Task Handle(RemoveGameFromCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), companyId!);

        company.RemoveGame(request.GameId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
