using Application.Common.Exceptions;
using Application.Common.Interface;
using Domain.Entities.Platforms;
using MediatR;

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
                ?? throw new NotFoundException(nameof(Platform), request.Id);

        platform.Update(request.Name, request.Description);

        await _repository.UpdateAsync(platform, cancellationToken);
    }
}
