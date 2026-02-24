using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Planner;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class StageConfiguration : IEntityTypeConfiguration<Stage>
{
    public void Configure(
        EntityTypeBuilder<Stage> builder
    )
    {
        builder.ToTable("stage");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Stage_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(50);
        builder
            .HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UX_Stage_Name");
    }
}