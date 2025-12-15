using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Role;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(
        EntityTypeBuilder<UserRole> builder
    )
    {
        builder.ToTable("user_role");
        builder
            .HasKey(e => new { e.UserID, e.RoleID })
            .HasName("PK_UserRole_UserId_RoleId");
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserID)
            .HasConstraintName("FK_UserRole_UserId");
        builder
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(e => e.RoleID)
            .HasConstraintName("FK_UserRole_RoleId");
    }
}