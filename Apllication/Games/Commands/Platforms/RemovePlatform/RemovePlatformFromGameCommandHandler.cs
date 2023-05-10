using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

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

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken);

        if (company is null)
            throw new NotFoundException(nameof(company), companyId!);

        var game = company!.Games.FirstOrDefault(game => game.Id == request.GameId);

        if (game is null)
            throw new NotFoundException(nameof(game), request.GameId);

        var platform = await _platformRepository.GetByIdAsync(request.PlatformId, cancellationToken);

        if (platform is null)
            throw new NotFoundException(nameof(platform), request.PlatformId);

        game.RemovePlatform(platform);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
