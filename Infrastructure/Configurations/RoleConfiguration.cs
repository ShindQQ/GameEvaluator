using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);

        builder.Property(role => role.Id)
            .HasConversion(
            roleId => roleId.Value,
            value => new RoleId(value));

        builder.HasMany(role => role.Users)
            .WithMany(user => user.Roles);

        builder.Property(role => role.Name)
          .HasMaxLength(50)
          .HasConversion(
              level => level.ToString(),
              roleName => (RoleType)Enum.Parse(typeof(RoleType), roleName))
              .IsUnicode(false);
    }
}
