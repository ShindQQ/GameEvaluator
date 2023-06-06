using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Genres.Commands.DeleteCommand;

public sealed class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
{
    private readonly IGenreRepository _repository;

    public DeleteGenreCommandHandler(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Genre with id {request.Id} was not found!");

        await _repository.DeleteAsync(genre, cancellationToken);
    }
}
