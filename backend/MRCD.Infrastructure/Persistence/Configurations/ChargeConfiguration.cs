using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Charge;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class ChargeConfiguration : IEntityTypeConfiguration<Charge>
{
    public void Configure(
        EntityTypeBuilder<Charge> builder
    )
    {
        builder.ToTable("charge");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Charge_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(30);
        builder
            .HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UX_Charge_Name");
        builder
            .Property(e => e.Amount)
            .HasPrecision(5, 2);
    }
}