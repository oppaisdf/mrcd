using MRCD.Domain.Common;

namespace MRCD.Domain.Role;

public sealed class Permission
{
    private Permission() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;

    public static Result<Permission> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Permission>.Failure("El nombre del permiso no puede estar vacío");
        return Result<Permission>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim()
        });
    }
}