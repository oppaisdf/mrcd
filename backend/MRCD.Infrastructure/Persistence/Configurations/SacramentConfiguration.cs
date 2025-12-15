using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Sacrament;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class SacramentConfiguration : IEntityTypeConfiguration<Sacrament>
{
    public void Configure(
        EntityTypeBuilder<Sacrament> builder
    )
    {
        builder.ToTable("sacrament");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Sacrament_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(16);
    }
}