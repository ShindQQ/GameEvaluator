﻿namespace Apllication.Common.Models.DTOs;

public sealed class PlatformDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
