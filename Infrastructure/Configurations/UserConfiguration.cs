using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

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
            .HasMaxLength(200);

        builder.OwnsMany(user => user.Roles, roleBuilder =>
        {
            roleBuilder.Property(role => role.Name).HasMaxLength(15);
        });

        builder.OwnsOne(user => user.BanState, banBuilder =>
        {
        });

        builder.HasOne(user => user.Company)
            .WithMany(company => company.Workers)
            .HasForeignKey(user => user.CompanyId);

        builder.HasMany(user => user.Comments).WithOne(comment => comment.User).OnDelete(DeleteBehavior.Cascade);
    }
}
