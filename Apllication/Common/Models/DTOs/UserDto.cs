namespace Application.Common.Models.DTOs;

public sealed class UserDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public List<GameDto> Games { get; init; } = new();

    public List<string> Roles { get; init; } = new();

    public string Company { get; init; } = string.Empty;
}
