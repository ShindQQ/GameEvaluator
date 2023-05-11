﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using Domain.Enums;
using MediatR;

namespace Application.Games.Commands.Genres.AddGenre;

public sealed class AddGenreToGameCommandHandler : IRequestHandler<AddGenreToGameCommand>
{
    private readonly IGenreRepository _genreRepository;

    private readonly ICompanyRepository _companyRepository;

    private readonly IApplicationDbContext _context;

    private readonly IUserService _userService;

    public AddGenreToGameCommandHandler(
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

    public async Task Handle(AddGenreToGameCommand request, CancellationToken cancellationToken)
    {
        var companyId = _userService.CompanyId;

        if (_userService.RoleType == RoleType.SuperAdmin)
            companyId = request.CompanyId;

        var company = await _companyRepository.GetByIdAsync(companyId!, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), companyId!);

        var game = company!.Games.FirstOrDefault(game => game.Id == request.GameId)
            ?? throw new NotFoundException(nameof(Game), request.GameId);

        var genre = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken)
            ?? throw new NotFoundException(nameof(Genre), request.GenreId);

        game.AddGenre(genre);

        await _context.SaveChangesAsync(cancellationToken);
    }
}