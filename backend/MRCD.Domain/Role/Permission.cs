using MRCD.Domain.Common;

namespace MRCD.Domain.Role;

public sealed class Permission : BaseEntity
{
    private Permission() { }
    public static Result<Permission> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Permission>.Failure("El nombre del permiso no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result<Permission>.Failure("El nombre del permiso no puede exceder los 30 caracteres");
        return Result<Permission>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim()
        });
    }
}