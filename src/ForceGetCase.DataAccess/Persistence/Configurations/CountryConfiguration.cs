using ForceGetCase.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForceGetCase.DataAccess.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasMany(p => p.Cities)
            .WithOne(p => p.Country)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
