using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Companies.Commands.Games.RemoveGame;

public sealed class RemoveGameFromCompanyCommandHandler : IRequestHandler<RemoveGameFromCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    public RemoveGameFromCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IApplicationDbContext context)
    {
        _companyRepository = companyRepository;
        _context = context;
    }

    public async Task Handle(RemoveGameFromCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), request.CompanyId);

        company.RemoveGame(request.GameId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
