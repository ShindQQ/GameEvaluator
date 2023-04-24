namespace Apllication.Common.Models;

public sealed class CompanyDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public List<GameDto> Games { get; init; } = new(); // how it can be nullable????

    public List<UserDto> Workers { get; init; } = new();
}
