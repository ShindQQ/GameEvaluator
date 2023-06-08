using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Platforms.Commands.DeleteCommand;

public sealed class DeletePlatformCommandHandler : IRequestHandler<DeletePlatformCommand>
{
    private readonly IPlatformRepository _repository;

    public DeletePlatformCommandHandler(IPlatformRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Platform with id {request.Id} was not found!");

        await _repository.DeleteAsync(platform, cancellationToken);
    }
}
