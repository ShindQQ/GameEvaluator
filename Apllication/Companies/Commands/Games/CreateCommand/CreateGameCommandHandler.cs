using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Apllication.Companies.Commands.Games.CreateCommand;

public sealed class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameId>
{
    private readonly IGameRepository _gameRepository;

    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateGameCommandHandler(
        IGameRepository gameRepository,
        ICompanyRepository companyRepository,
        IApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _gameRepository = gameRepository;
        _companyRepository = companyRepository;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GameId> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var claim = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId");
        var companyId = claim is not null ? new CompanyId(Guid.Parse(claim.Value)) : request.CompanyId!;

        var company = await _companyRepository.GetByIdAsync(companyId, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(Company), companyId);

        var game = Game.Create(request.Name!, request.Description!);

        company.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
