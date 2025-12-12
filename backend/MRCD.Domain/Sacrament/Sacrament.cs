using MRCD.Domain.Common;

namespace MRCD.Domain.Sacrament;

public sealed class Sacrament
{
    private Sacrament() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;

    public static Result<Sacrament> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Sacrament>.Failure("El nombre del grado académico no puede estar vacío");
        return Result<Sacrament>.Success(new()
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
        if (name.Trim().Equals(Name))
            return Result.Failure("El nombre ya está en uso");
        Name = name.Trim();
        return Result.Success();
    }
}