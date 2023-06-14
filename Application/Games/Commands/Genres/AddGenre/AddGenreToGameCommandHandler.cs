﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Enums;
using MediatR;
using System.Net;

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
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Company with id {companyId} was not found!");

        var game = company!.Games.FirstOrDefault(game => game.Id == request.GameId)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Game with id {request.GameId} was not found!");

        var genre = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Genre with id {request.GenreId} was not found!");

        game.AddGenre(genre);

        await _context.SaveChangesAsync(cancellationToken);
    }
}