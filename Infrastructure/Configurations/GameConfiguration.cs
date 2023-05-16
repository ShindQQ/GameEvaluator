using Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(game => game.Id);

        builder.Property(game => game.Id).HasConversion(
            gameId => gameId.Value,
            value => new GameId(value));

        builder.Property(game => game.Name)
            .HasMaxLength(20);

        builder.Property(game => game.Description)
            .HasMaxLength(200);

        builder.HasMany(game => game.Companies)
            .WithMany(company => company.Games);

        builder.HasMany(game => game.Genres).WithMany(genre => genre.Games);

        builder.HasMany(game => game.Platforms).WithMany(platform => platform.Games);

        builder.HasMany(game => game.Comments).WithOne(comment => comment.Game).OnDelete(DeleteBehavior.Cascade);
    }
}
