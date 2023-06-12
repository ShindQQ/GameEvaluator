using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Games;

namespace Application.Common.Interfaces.Repositories;

public interface IGameRepository : IBaseRepository<Game, GameId>
{
}
