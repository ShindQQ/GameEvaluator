using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Games.Commands.Genres.RemoveGenre;

public sealed class RemoveGenreFromGameCommandHandler : IRequestHandler<RemoveGenreFromGameCommand>
{
    private readonly IGenreRepository _genreRepository;

    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public RemoveGenreFromGameCommandHandler(
        IApplicationDbContext context,
        IGenreRepository genreRepository,
        ICompanyRepository companyRepository,
        IUserService userService)
    {
        _context = context;
        _genreRepository = genreRepository;
        _companyRepository = companyRepository;
        _userService = userService;
    }

    public async Task Handle(RemoveGenreFromGameCommand request, CancellationToken cancellationToken)
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

        var genre = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken);

        if (genre is null)
            throw new NotFoundException(nameof(genre), request.GenreId);

        game.RemoveGenre(genre);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
