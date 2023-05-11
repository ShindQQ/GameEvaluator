﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Enums;
using MediatR;

namespace Application.Companies.Commands.Games.RemoveGame;

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

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), companyId!);

        company.RemoveGame(request.GameId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
