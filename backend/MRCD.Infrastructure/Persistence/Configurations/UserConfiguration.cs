using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(
        EntityTypeBuilder<User> builder
    )
    {
        builder.ToTable("user");
        builder
            .HasKey(u => u.ID)
            .HasName("PK_User_Id");
        builder
            .Property(u => u.ID)
            .ValueGeneratedNever();
        builder
            .Property(u => u.Username)
            .HasMaxLength(10);
        builder
            .HasIndex(e => e.Username)
            .IsUnique()
            .HasDatabaseName("UX_User_Username");
    }
}