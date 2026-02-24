using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Degree;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class DegreeConfiguration : IEntityTypeConfiguration<Degree>
{
    public void Configure(
        EntityTypeBuilder<Degree> builder
    )
    {
        builder.ToTable("degree");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Degree_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(30);
        builder
            .HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UX_Degree_Name");
    }
}