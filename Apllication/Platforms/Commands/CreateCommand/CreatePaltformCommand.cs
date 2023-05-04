using Domain.Entities.Platforms;
using MediatR;

namespace Application.Platforms.Commands.CreateCommand;

public record CreatePaltformCommand : IRequest<PlatformId>
{
    public string? Name { get; init; }

    public string? Description { get; init; }
}
