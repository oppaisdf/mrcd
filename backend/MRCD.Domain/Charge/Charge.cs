using MRCD.Domain.Common;

namespace MRCD.Domain.Charge;

public sealed class Charge
{
    private Charge() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal Amount { get; private set; }

    public static Result<Charge> Create(
        string name,
        decimal amount
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Charge>.Failure("El nombre del cobro no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result<Charge>.Failure("El nombre del cobro  no puede exceder los 30 caracteres");
        if (amount < 1 || amount > 500)
            return Result<Charge>.Failure("El monto del cobro debe estar entre 1-500");
        return Result<Charge>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim(),
            Amount = amount
        });
    }

    public Result SetName(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("El nombre del grado académico no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result.Failure("El nombre del cobro  no puede exceder los 30 caracteres");
        if (name.Trim().Equals(Name))
            return Result.Failure("El nombre ya está en uso");
        Name = name.Trim();
        return Result.Success();
    }

    public Result SetAmount(
        decimal amount
    )
    {
        if (amount < 1 || amount > 500)
            return Result.Failure("El monto del cobro debe estar entre 1-500");
        Amount = amount;
        return Result.Success();
    }
}