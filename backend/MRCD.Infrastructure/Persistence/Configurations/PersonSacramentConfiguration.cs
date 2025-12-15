using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Person;
using MRCD.Domain.Sacrament;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class PersonSacramentConfiguration : IEntityTypeConfiguration<PersonSacrament>
{
    public void Configure(
        EntityTypeBuilder<PersonSacrament> builder
    )
    {
        builder.ToTable("person_sacrament");
        builder
            .HasKey(e => new { e.PersonId, e.SacramentId })
            .HasName("PK_PersonSacrament_PersonId_SacramentId");
        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .HasConstraintName("FK_PersonSacrament_PersonId");
        builder
            .HasOne<Sacrament>()
            .WithMany()
            .HasForeignKey(e => e.SacramentId)
            .HasConstraintName("FK_PersonSacrament_SacramentId");
    }
}