using Domain.Entities.Platforms;
using MediatR;

namespace Application.Platforms.Commands.DeleteCommand;

public record DeletePlatformCommand(PlatformId Id) : IRequest;
