using Apllication.Common.Mappings;
using Domain.Entities.Companies;

namespace Apllication.Common.Models;

public sealed class CompanyDto : IMapFrom<Company>
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public List<GameDto> Games { get; init; } = new();

    public List<UserDto> Workers { get; init; } = new();
}
