using MRCD.Domain.Common;

namespace MRCD.Domain.Degree;

public sealed class Degree
{
    private Degree() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;

    public static Result<Degree> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Degree>.Failure("El nombre del grado académico no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result<Degree>.Failure("El nombre del grado académico no puede exceder los 30 caracteres");
        return Result<Degree>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim()
        });
    }

    public Result SetName(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("El nombre del grado académico no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result.Failure("El nombre del grado académico no puede exceder los 30 caracteres");
        if (name.Trim().Equals(Name))
            return Result.Failure("El nombre ya está en uso");
        Name = name.Trim();
        return Result.Success();
    }
}