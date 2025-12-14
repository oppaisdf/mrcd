using MRCD.Domain.Common;

namespace MRCD.Domain.Planner;

public sealed class Activity
{
    private Activity() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;
    public DateOnly Date { get; private set; }

    public static Result<Activity> Create(
        string name,
        DateOnly date
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Activity>.Failure("El nombre de la actividad no puede estar vacío");
        if (name.Trim().Length > 50)
            return Result<Activity>.Failure("El nombre de la actividad no puede exceder los 50 caracteres");
        var now = DateTime.UtcNow.AddHours(-6);
        if (date.Year != now.Year)
            return Result<Activity>.Failure($"La actividad debe estar dentro del año{now.Year}");
        return Result<Activity>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim(),
            Date = date
        });
    }
}