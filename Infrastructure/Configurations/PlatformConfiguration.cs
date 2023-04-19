using Domain.Entities.Platforms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.HasKey(platform => platform.Id);

        builder.Property(platform => platform.Id)
            .HasConversion(
            platformId => platformId.Value,
            value => new PlatformId(value));

        builder.Property(platform => platform.Name)
          .HasMaxLength(50)
          .HasConversion(
              platformType => platformType.ToString(),
              platformName => (PlatformType)Enum.Parse(typeof(PlatformType), platformName))
              .IsUnicode(false);

        builder.HasMany(platform => platform.Games)
            .WithMany(game => game.Platforms);
    }
}
