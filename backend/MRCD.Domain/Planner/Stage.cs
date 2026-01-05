using MRCD.Domain.Common;

namespace MRCD.Domain.Planner;

public sealed class Stage : BaseEntity
{
    private Stage() { }

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
}