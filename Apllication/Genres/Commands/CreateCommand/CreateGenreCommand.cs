using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Genres.Commands.CreateCommand;

public record CreateGenreCommand : IRequest<GenreId>
{
    public string? Name { get; init; }

    public string? Description { get; init; }
}
