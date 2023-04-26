using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Genres.Commands.UpdateCommand;

public sealed class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
{
    private readonly IGenreRepository _repository;

    public UpdateGenreCommandHandler(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (genre == null)
            throw new NotFoundException(nameof(Genre), request.Id);

        genre.Update(request.Name, request.Description);

        await _repository.UpdateAsync(genre, cancellationToken);
    }
}
