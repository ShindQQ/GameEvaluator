using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Companies.Commands.Games.CreateCommand;

public sealed class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameId>
{
    private readonly IGameRepository _gameRepository;

    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    public CreateGameCommandHandler(
        IGameRepository gameRepository,
        ICompanyRepository companyRepository,
        IApplicationDbContext context)
    {
        _gameRepository = gameRepository;
        _companyRepository = companyRepository;
        _context = context;
    }

    public async Task<GameId> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(Company), request.CompanyId);

        var game = Game.Create(request.Name!, request.Description!);

        company.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
