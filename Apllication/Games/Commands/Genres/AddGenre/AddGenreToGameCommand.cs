using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Games.Commands.Genres.AddGenre;

public record AddGenreToGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public CompanyId? CompanyId { get; init; }

    public GenreId GenreId { get; init; } = null!;
}
