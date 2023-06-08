using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;
using System.Net;

namespace Application.Games.Commands.Platforms.RemovePlatform;

public sealed class RemovePlatformFromGameCommandHandler : IRequestHandler<RemovePlatformFromGameCommand>
{
    private readonly IPlatformRepository _platformRepository;

    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public RemovePlatformFromGameCommandHandler(
        IApplicationDbContext context,
        IPlatformRepository platformRepository,
        ICompanyRepository companyRepository,
        IUserService userService)
    {
        _context = context;
        _platformRepository = platformRepository;
        _companyRepository = companyRepository;
        _userService = userService;
    }

    public async Task Handle(RemovePlatformFromGameCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Company with id {companyId} was not found!");

        var game = company!.Games.FirstOrDefault(game => game.Id == request.GameId)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Game with id {request.GameId} was not found!");

        var platform = await _platformRepository.GetByIdAsync(request.PlatformId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Platform with id {request.PlatformId} was not found!");

        game.RemovePlatform(platform);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
