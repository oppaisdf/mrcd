using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Planner;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class ActivityConfigurarion : IEntityTypeConfiguration<Activity>
{
    public void Configure(
        EntityTypeBuilder<Activity> builder
    )
    {
        builder.ToTable("activity");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Activity_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(50);
        builder
            .HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UX_Activity_Name");
    }
}