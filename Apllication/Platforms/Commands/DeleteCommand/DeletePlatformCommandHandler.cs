using Application.Common.Exceptions;
using Application.Common.Interface;
using Domain.Entities.Platforms;
using MediatR;

namespace Application.Platforms.Commands.DeleteCommand
{
    public sealed class DeletePlatformCommandHandler : IRequestHandler<DeletePlatformCommand>
    {
        private readonly IPlatformRepository _repository;

        public DeletePlatformCommandHandler(IPlatformRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
        {
            var platform = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (platform == null)
                throw new NotFoundException(nameof(Platform), request.Id);

            await _repository.DeleteAsync(platform, cancellationToken);
        }
    }
}
