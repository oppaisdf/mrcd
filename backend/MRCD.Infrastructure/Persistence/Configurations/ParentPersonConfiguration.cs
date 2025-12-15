using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Parent;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class ParentPersonConfiguration : IEntityTypeConfiguration<ParentPerson>
{
    public void Configure(
        EntityTypeBuilder<ParentPerson> builder
    )
    {
        builder.ToTable("parent_person");
        builder
            .HasKey(e => new { e.ParentId, e.PersonId, e.IsParent })
            .HasName("PK_ParentPerson_ParentId_PersonId_IsParent");
        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .HasConstraintName("FK_ParentPerson_PersonId_Person_ID");
        builder
            .HasOne<Parent>()
            .WithMany()
            .HasForeignKey(e => e.ParentId)
            .HasConstraintName("FK_ParentPerson_ParentId_Parent_ID");
    }
}