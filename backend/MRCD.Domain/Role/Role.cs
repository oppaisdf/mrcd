using MRCD.Domain.Common;

namespace MRCD.Domain.Role;

public sealed class Role
{
    private Role() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;

    public static Result<Role> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Role>.Failure("El nombre del rol no puede venir vacío");
        if (name.Trim().Length > 3)
            return Result<Role>.Failure("El nombre del rol no puede exceder los tres caracteres");
        return Result<Role>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim()
        });
    }
}