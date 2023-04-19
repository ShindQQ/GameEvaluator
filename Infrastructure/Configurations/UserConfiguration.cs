using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasConversion(
            userId => userId.Value,
            value => new UserId(value));

        builder.Property(user => user.Name)
            .HasMaxLength(20);

        builder.Property(user => user.Email)
            .HasMaxLength(30);

        builder.Property(user => user.Password)
            .HasMaxLength(60);

        builder.HasMany(user => user.Roles)
            .WithMany(role => role.Users);

        builder.HasOne(user => user.Company)
            .WithMany(company => company.Workers)
            .HasForeignKey(user => user.CompanyId);
    }
}
