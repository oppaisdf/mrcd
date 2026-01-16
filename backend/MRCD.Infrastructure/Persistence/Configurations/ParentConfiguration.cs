using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Parent;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(
        EntityTypeBuilder<Parent> builder
    )
    {
        builder.ToTable("parent");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Parent_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(80);
        builder
            .Property(e => e.NormalizedName)
            .HasMaxLength(80);
    }
}