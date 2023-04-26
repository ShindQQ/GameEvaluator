using Apllication.Common.Exceptions;
using Apllication.Common.Interfaces.Repositories;
using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Genres.Commands.DeleteCommand;

public sealed class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
{
    private readonly IGenreRepository _repository;

    public DeleteGenreCommandHandler(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (genre == null)
            throw new NotFoundException(nameof(Genre), request.Id);

        await _repository.DeleteAsync(genre, cancellationToken);
    }
}
