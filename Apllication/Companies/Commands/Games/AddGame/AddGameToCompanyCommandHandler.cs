using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR;

namespace Apllication.Companies.Commands.Games.AddGame;

public sealed class AddGameToCompanyCommandHandler : IRequestHandler<AddGameToCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IGameRepository _gameRepository;

    private readonly IApplicationDbContext _context;

    public AddGameToCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IGameRepository gameRepository,
        IApplicationDbContext context)
    {
        _companyRepository = companyRepository;
        _gameRepository = gameRepository;
        _context = context;
    }

    public async Task Handle(AddGameToCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);

        if (company is null)
            throw new NullReferenceException(nameof(company));

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            throw new NullReferenceException(nameof(game));

        company.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
