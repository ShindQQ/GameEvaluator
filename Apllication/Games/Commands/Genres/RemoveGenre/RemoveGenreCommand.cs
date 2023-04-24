using Domain.Entities.Games;
using Domain.Enums;
using MediatR;

namespace Apllication.Games.Commands.Genres.RemoveGenre;

public record RemoveGenreCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public GenreType GenreType { get; init; }
}
