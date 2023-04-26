﻿using Domain.Entities.Companies;

namespace Apllication.Common.Models;

public record AuthModel
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public CompanyId? CompanyId { get; init; }
}
