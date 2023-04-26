using Domain.Entities.Games;
using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Games.Commands.Genres.RemoveGenre;

public record RemoveGenreFromGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public GenreId GenreId { get; init; } = null!;
}
