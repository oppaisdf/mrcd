using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.AccountingMovement;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class AccountingMovementConfiguration : IEntityTypeConfiguration<AccountingMovement>
{
    public void Configure(
        EntityTypeBuilder<AccountingMovement> builder
    )
    {
        builder.ToTable("accounting_movement");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_AccountingMovement_ID");
        builder
            .Property(e => e.Amount)
            .HasPrecision(5, 2);
        builder
            .Property(e => e.Description)
            .HasMaxLength(50);
    }
}