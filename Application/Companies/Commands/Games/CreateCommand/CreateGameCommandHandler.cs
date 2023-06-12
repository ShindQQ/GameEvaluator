using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Games;
using Domain.Enums;
using MediatR;
using System.Net;

namespace Application.Companies.Commands.Games.CreateCommand;

public sealed class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameId>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public CreateGameCommandHandler(
        ICompanyRepository companyRepository,
        IApplicationDbContext context,
        IUserService userService)
    {
        _companyRepository = companyRepository;
        _context = context;
        _userService = userService;
    }

    public async Task<GameId> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Company with id {companyId} was not found!");

        var game = Game.Create(request.Name!, request.Description!);

        company.AddGame(game);

        await _context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
