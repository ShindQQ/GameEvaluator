using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Apllication.Companies.Commands.Workers.RemoveWorker;

public sealed class RemoveWorkerCommandHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveWorkerCommandHandler(
        ICompanyRepository companyRepository,
        IApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _companyRepository = companyRepository;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(RemoveWorkerCommand request, CancellationToken cancellationToken)
    {
        var claim = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId");
        var companyId = claim is not null ? new CompanyId(Guid.Parse(claim.Value)) : request.CompanyId!;

        var company = await _companyRepository.GetByIdAsync(companyId, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), companyId);

        company.RemoveWorker(request.UserId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
