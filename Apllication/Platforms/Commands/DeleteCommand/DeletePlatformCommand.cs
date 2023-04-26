using Domain.Entities.Platforms;
using MediatR;

namespace Apllication.Platforms.Commands.DeleteCommand;

public record DeletePlatformCommand(PlatformId Id) : IRequest;
