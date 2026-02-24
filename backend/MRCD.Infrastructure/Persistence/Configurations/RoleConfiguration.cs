using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(
        EntityTypeBuilder<Role> builder
    )
    {
        builder.ToTable("role");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Role_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(3);
        builder
            .HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UX_Role_Name");
    }
}