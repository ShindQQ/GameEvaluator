using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Platforms.Commands.UpdateCommand;

public record UpdatePlatformCommand : IRequest
{
    public PlatformId Id { get; init; } = null!;

    public string? Name { get; init; }

    public string? Description { get; init; }
}
