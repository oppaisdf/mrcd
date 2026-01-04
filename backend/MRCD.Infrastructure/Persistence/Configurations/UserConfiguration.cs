using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Application.Abstracts.Security;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration(
    IEncryptionService service
) : IEntityTypeConfiguration<User>
{
    private readonly IEncryptionService _service = service;

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
            .Property(u => u.Password)
            .HasColumnType("longtext")
            .HasConversion(
                v => _service.Encrypt(v),
                v => _service.Decrypt(v)!
            );
    }
}