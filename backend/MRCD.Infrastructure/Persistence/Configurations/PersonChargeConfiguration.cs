using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Charge;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class PersonChargeConfiguration : IEntityTypeConfiguration<PersonCharge>
{
    public void Configure(
        EntityTypeBuilder<PersonCharge> builder
    )
    {
        builder.ToTable("person_charge");
        builder
            .HasKey(e => new { e.PersonId, e.ChargeId })
            .HasName("PK_PersonCharge_PersonId_ChargeId");
        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .HasConstraintName("FK_PersonCharge_PersonId");
        builder
            .HasOne<Charge>()
            .WithMany()
            .HasForeignKey(e => e.ChargeId)
            .HasConstraintName("FK_PersonCharge_ChargeId");
    }
}