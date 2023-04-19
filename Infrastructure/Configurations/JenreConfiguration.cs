using Domain.Entities.Jenres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class JenreConfiguration : IEntityTypeConfiguration<Jenre>
{
    public void Configure(EntityTypeBuilder<Jenre> builder)
    {
        builder.HasKey(jenre => jenre.Id);

        builder.Property(jenre => jenre.Id)
            .HasConversion(
            jenreId => jenreId.Value,
            value => new JenreId(value));


        builder.Property(jenre => jenre.Name)
            .HasMaxLength(20);

        builder.HasMany(jenre => jenre.Games)
            .WithMany(game => game.Jenres);
    }
}
