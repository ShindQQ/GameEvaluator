using Application.Common.Interfaces.Repositories;
using Domain.Entities.Platforms;
using MediatR;

namespace Application.Platforms.Commands.CreateCommand;

public sealed class CreatePlatformCommandHandler : IRequestHandler<CreatePlatformCommand, PlatformId>
{
    private readonly IPlatformRepository _repository;

    public CreatePlatformCommandHandler(IPlatformRepository repository)
    {
        _repository = repository;
    }

    public async Task<PlatformId> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = Platform.Create(request.Name!, request.Description!);

        await _repository.AddAsync(platform, cancellationToken);

        return platform.Id;
    }
}
