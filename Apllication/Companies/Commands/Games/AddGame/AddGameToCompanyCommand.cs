﻿using Domain.Entities.Companies;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Companies.Commands.Games.AddGame;

public record AddGameToCompanyCommand : IRequest
{
    public CompanyId CompanyId { get; init; } = null!;

    public GameId GameId { get; init; } = null!;
}