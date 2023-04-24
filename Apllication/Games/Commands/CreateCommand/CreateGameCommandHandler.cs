using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Commands.CreateCommand;

public sealed class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameId>
{
    private readonly IApplicationDbContext _context;

    private readonly ICompanyRepository _companyRepository;

    public CreateGameCommandHandler(IApplicationDbContext context,
        ICompanyRepository companyRepository)
    {
        _context = context;
        _companyRepository = companyRepository;
    }

    public async Task<GameId> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var game = Game.Create(request.Name!, request.Description!);

        var company = await _companyRepository.GetByIdAsync(request.CompanyId);

        if (company == null)
            throw new ArgumentNullException(nameof(company));

        company.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
