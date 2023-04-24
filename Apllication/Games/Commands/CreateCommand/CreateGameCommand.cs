﻿using Domain.Entities.Companies;
using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Commands.CreateCommand;

public record CreateGameCommand : IRequest<GameId>
{
    public CompanyId CompanyId { get; init; } = null!;

    public string? Name { get; init; }

    public string? Description { get; init; }
}
