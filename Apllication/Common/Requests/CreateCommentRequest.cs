﻿using Domain.Entities.Games;
using Domain.Entities.Users;

namespace Application.Common.Requests;

public sealed class CreateCommentRequest
{
    public UserId UserId { get; init; } = null!;

    public GameId GameId { get; init; } = null!;

    public string Text { get; init; } = string.Empty;
}