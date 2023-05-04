using Application.Common.Interface;
using Domain.Entities.Platforms;
using MediatR;

namespace Application.Platforms.Commands.CreateCommand;

public sealed class CreatePaltformCommandHandler : IRequestHandler<CreatePaltformCommand, PlatformId>
{
    private readonly IPlatformRepository _repository;

    public CreatePaltformCommandHandler(IPlatformRepository repository)
    {
        _repository = repository;
    }

    public async Task<PlatformId> Handle(CreatePaltformCommand request, CancellationToken cancellationToken)
    {
        var platform = Platform.Create(request.Name!, request.Description!);

        await _repository.AddAsync(platform, cancellationToken);

        return platform.Id;
    }
}
