using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Platforms.Commands.UpdateCommand;

public sealed class UpdatePlatformCommandHandler : IRequestHandler<UpdatePlatformCommand>
{
    private readonly IPlatformRepository _repository;

    public UpdatePlatformCommandHandler(IPlatformRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Platform with id {request.Id} was not found!");

        platform.Update(request.Name, request.Description);

        await _repository.UpdateAsync(platform, cancellationToken);
    }
}
