using Application.Common.Interfaces.Repositories;
using Domain.Entities.Genres;
using MediatR;

namespace Application.Genres.Commands.CreateCommand;

public sealed class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreId>
{
    private readonly IGenreRepository _repository;

    public CreateGenreCommandHandler(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task<GenreId> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = Genre.Create(request.Name!, request.Description!);

        await _repository.AddAsync(genre, cancellationToken);

        return genre.Id;
    }
}
