using Apllication.Common.Interfaces.Repositories.Base;
using Domain.Entities.Games;

namespace Apllication.Common.Interfaces.Repositories;

public interface IGameRepository : IBaseRepository<Game, GameId>
{
}
