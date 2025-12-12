using MRCD.Domain.Common;

namespace MRCD.Domain.AccountingMovement;

public sealed class AccountingMovement
{
    private AccountingMovement() { }
    public Guid ID { get; private set; }
    public string Description { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public DateOnly Date { get; private set; }

    public static Result<AccountingMovement> Create(
        string description,
        decimal amount
    )
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result<AccountingMovement>.Failure("La descripción no puede estar vacía");
        if (amount == 0)
            return Result<AccountingMovement>.Failure("El monto debe ser distinto de cero");
        if (amount < 5000 || amount > 5000)
            return Result<AccountingMovement>.Failure("El monto no puede ser mayor o menor a 5000");
        return Result<AccountingMovement>.Success(new()
        {
            ID = Guid.NewGuid(),
            Description = description.Trim(),
            Amount = amount,
            Date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6))
        });
    }
}