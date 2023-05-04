using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Enums;
using MediatR;

namespace Application.Companies.Commands.Games.CreateCommand;

public sealed class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameId>
{
    private readonly IGameRepository _gameRepository;

    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public CreateGameCommandHandler(
        IGameRepository gameRepository,
        ICompanyRepository companyRepository,
        IApplicationDbContext context,
        IUserService userService)
    {
        _gameRepository = gameRepository;
        _companyRepository = companyRepository;
        _context = context;
        _userService = userService;
    }

    public async Task<GameId> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(Company), companyId!);

        var game = Game.Create(request.Name!, request.Description!);

        company.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
