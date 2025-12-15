using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Document;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class DocumentConfigurarion : IEntityTypeConfiguration<Document>
{
    public void Configure(
        EntityTypeBuilder<Document> builder
    )
    {
        builder.ToTable("document");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Document_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .Property(e => e.Name)
            .HasMaxLength(30);
    }
}