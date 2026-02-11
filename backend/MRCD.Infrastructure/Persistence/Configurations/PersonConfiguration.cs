using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Degree;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
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
            .HasIndex(e => e.NormalizedName)
            .IsUnique()
            .HasDatabaseName("UX_Person_NormalizedName");
        builder
            .Property(e => e.Parish)
            .HasMaxLength(30);
        builder
            .HasOne<Degree>()
            .WithMany()
            .HasForeignKey(e => e.LastDegreeId)
            .HasConstraintName("FK_Person_DegreeId");
    }
}