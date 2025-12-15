using MRCD.Domain.Common;

namespace MRCD.Domain.Planner;

public sealed class Stage
{
    private Stage() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;

    public static Result<Stage> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Stage>.Failure("El nombre de la fase de actividad no puede estar vacío");
        if (name.Trim().Length > 50)
            return Result<Stage>.Failure("El nombre de la fase de actividad no puede exceder los 50 caracteres");
        return Result<Stage>.Success(new()
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
            return Result.Failure("El nombre de la fase de actividad no puede estar vacío");
        if (name.Trim().Length > 50)
            return Result.Failure("El nombre de la fase de actividad no puede exceder los 50 caracteres");
        if (name.Trim().Equals(Name))
            return Result.Failure("El nombre ya está en uso");
        Name = name.Trim();
        return Result.Success();
    }
}