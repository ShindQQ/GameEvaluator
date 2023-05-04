using Application.Common.Interfaces.Repositories;
using Application.Infrastructure;
using Domain.Entities.Genres;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class GenreRepository
: BaseRepository<Genre, GenreId>,
IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<Genre>> GetAsync()
        => Task.FromResult(Context.Genres
            .AsQueryable());

    public override async Task<Genre?> GetByIdAsync(GenreId id, CancellationToken cancellationToken)
        => await Context.Genres
            .FirstOrDefaultAsync(genre => genre.Id == id, cancellationToken);
}
