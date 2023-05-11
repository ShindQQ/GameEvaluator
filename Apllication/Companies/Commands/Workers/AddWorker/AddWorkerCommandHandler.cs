﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Users;
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

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), companyId!);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        company.AddWorker(user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
