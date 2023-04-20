using Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
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

        builder.OwnsMany(game => game.Jenres, jenreBuilder =>
        {
            jenreBuilder.Property(jenre => jenre.Name).HasMaxLength(15);
        });

        builder.OwnsMany(game => game.Platforms, platformBuilder =>
        {
            platformBuilder.Property(platform => platform.Name).HasMaxLength(15);
        });
    }
}
