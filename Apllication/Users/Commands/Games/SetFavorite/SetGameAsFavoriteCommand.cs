﻿using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;

namespace Application.Users.Commands.Games.SetFavorite;

public record SetGameAsFavoriteCommand : IRequest
{
    public UserId UserId { get; init; } = null!;

    public GameId GameId { get; init; } = null!;
}
