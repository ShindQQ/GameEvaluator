using Domain.Entities.Platforms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class PlatormConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.ToTable("Platforms");

        builder.HasKey(platform => platform.Id);

        builder.Property(platform => platform.Id).HasConversion(
            platformId => platformId.Value,
            value => new PlatformId(value));

        builder.Property(platform => platform.Name).HasMaxLength(20);

        builder.Property(platform => platform.Description).HasMaxLength(200);

        builder.HasMany(platform => platform.Games).WithMany(game => game.Platforms);
    }
}
