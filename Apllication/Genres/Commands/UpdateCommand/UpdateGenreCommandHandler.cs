using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Application.Genres.Commands.UpdateCommand;

public sealed class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
{
    private readonly IGenreRepository _repository;

    public UpdateGenreCommandHandler(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new StatusCodeException(HttpStatusCode.NotFound, $"Genre with id {request.Id} was not found!");

        genre.Update(request.Name, request.Description);

        await _repository.UpdateAsync(genre, cancellationToken);
    }
}
