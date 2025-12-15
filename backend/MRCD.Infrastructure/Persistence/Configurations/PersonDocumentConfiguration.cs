using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Document;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class PersonDocumentConfiguration : IEntityTypeConfiguration<PersonDocument>
{
    public void Configure(
        EntityTypeBuilder<PersonDocument> builder
    )
    {
        builder.ToTable("person_document");
        builder
            .HasKey(e => new { e.PersonId, e.DocumentId })
            .HasName("PK_PersonDocument_PersonId_DocumentId");
        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .HasConstraintName("FK_PersonDocument_PersonId");
        builder
            .HasOne<Document>()
            .WithMany()
            .HasForeignKey(e => e.DocumentId)
            .HasConstraintName("FK_PersonDocument_DocumentId");
    }
}