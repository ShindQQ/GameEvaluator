using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(company => company.Id);

        builder.Property(company => company.Id).HasConversion(
            companyId => companyId.Value,
            value => new CompanyId(value));

        builder.Property(company => company.Name).HasMaxLength(20);

        builder.Property(company => company.Description).HasMaxLength(200);

        builder.HasMany(company => company.Games).WithMany(game => game.Companies);

        builder.HasMany(company => company.Workers).WithOne(worker => worker.Company).OnDelete(DeleteBehavior.NoAction);
    }
}
