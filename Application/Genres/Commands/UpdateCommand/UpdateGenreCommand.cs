using Domain.Entities.Genres;
using MediatR;

namespace Application.Genres.Commands.UpdateCommand;

public record UpdateGenreCommand : IRequest
{
    public GenreId Id { get; init; } = null!;

    public string? Name { get; init; }

    public string? Description { get; init; }
}
