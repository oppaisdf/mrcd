using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(
        EntityTypeBuilder<RolePermission> builder
    )
    {
        builder.ToTable("role_permission");
        builder
            .HasKey(e => new { e.RoleID, e.PermissionID })
            .HasName("PK_RolePermission_RoleId_PermissionId");
        builder
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(e => e.RoleID)
            .HasConstraintName("FK_RolePermission_RoleId");
        builder
            .HasOne<Permission>()
            .WithMany()
            .HasForeignKey(e => e.PermissionID)
            .HasConstraintName("FK_RolePermission_PermissionId");
    }
}