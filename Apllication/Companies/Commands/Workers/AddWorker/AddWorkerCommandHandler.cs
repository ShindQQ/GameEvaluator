using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Apllication.Companies.Commands.Workers.AddWorker;

public sealed class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IUserRepository _userRepository;

    private readonly IApplicationDbContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddWorkerCommandHandler(
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        IApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var claim = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId");
        var companyId = claim is not null ? new CompanyId(Guid.Parse(claim.Value)) : request.CompanyId!;

        var company = await _companyRepository.GetByIdAsync(companyId, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), companyId);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.UserId);

        company.AddWorker(user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
