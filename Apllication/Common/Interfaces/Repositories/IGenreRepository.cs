using Apllication.Common.Interfaces.Repositories.Base;
using Domain.Entities.Genres;

namespace Apllication.Common.Interfaces.Repositories;

public interface IGenreRepository : IBaseRepository<Genre, GenreId>
{
}
