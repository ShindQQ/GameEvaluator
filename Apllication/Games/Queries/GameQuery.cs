using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Queries;

public record GameQuery(GameId Id) : IRequest<List<Game>>;
