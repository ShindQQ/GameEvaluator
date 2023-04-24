using Domain.Entities.Games;
using Domain.Enums;
using MediatR;

namespace Apllication.Games.Commands.Genres.AddGenre;

public record AddGenreCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public GenreType GenreType { get; init; }
}
