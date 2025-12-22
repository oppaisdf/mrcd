using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Application.Abstracts.Security;
using MRCD.Domain.Degree;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class PersonConfiguration(
    IEncryptionService service
) : IEntityTypeConfiguration<Person>
{
    private readonly IEncryptionService _service = service;

    public void Configure(
        EntityTypeBuilder<Person> builder
    )
    {
        builder.ToTable("person");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Person_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(65);
        builder
            .Property(e => e.NormalizedName)
            .HasMaxLength(65);
        builder
            .Property(e => e.Parish)
            .HasMaxLength(30);
        builder
            .Property(e => e.Address)
            .HasColumnType("longtext")
            .HasConversion(
                v => _service.Encrypt(v),
                v => _service.Decrypt(v)
            );
        builder
            .Property(e => e.Phone)
            .HasColumnType("longtext")
            .HasConversion(
                v => _service.Encrypt(v),
                v => _service.Decrypt(v)
            );
        builder
            .HasOne<Degree>()
            .WithMany()
            .HasForeignKey(e => e.LastDegreeId)
            .HasConstraintName("FK_Person_DegreeId");
    }
}