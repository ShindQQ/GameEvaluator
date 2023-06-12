using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Genres;

namespace Application.Common.Interfaces.Repositories;

public interface IGenreRepository : IBaseRepository<Genre, GenreId>
{
}
