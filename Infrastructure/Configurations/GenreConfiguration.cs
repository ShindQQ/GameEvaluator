using Domain.Entities.Genres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(genre => genre.Id);

        builder.Property(genre => genre.Id).HasConversion(
            genreId => genreId.Value,
            value => new GenreId(value));

        builder.Property(genre => genre.Name).HasMaxLength(20);

        builder.Property(genre => genre.Description).HasMaxLength(200);

        builder.HasMany(genre => genre.Games).WithMany(game => game.Genres);
    }
}
