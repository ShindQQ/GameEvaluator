using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using MediatR;

namespace Application.Games.Commands.Genres.RemoveGenre;

public record RemoveGenreFromGameCommand : IRequest
{
    public GameId GameId { get; init; } = null!;

    public CompanyId? CompanyId { get; init; }

    public GenreId GenreId { get; init; } = null!;
}
