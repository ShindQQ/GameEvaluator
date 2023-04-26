﻿using Apllication.Common.Exceptions;
using Apllication.Common.Interface;
using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Platforms.Commands.UpdateCommand;

public sealed class UpdatePlatformCommandHandler : IRequestHandler<UpdatePlatformCommand>
{
    private readonly IPlatformRepository _repository;

    public UpdatePlatformCommandHandler(IPlatformRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (platform == null)
            throw new NotFoundException(nameof(Genre), request.Id);

        platform.Update(request.Name, request.Description);

        await _repository.UpdateAsync(platform, cancellationToken);
    }
}
