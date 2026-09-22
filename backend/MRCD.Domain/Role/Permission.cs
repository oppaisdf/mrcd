using MRCD.Domain.Common;
using System.Text.RegularExpressions;

namespace MRCD.Domain.Role;

public sealed partial class Permission : BaseEntity
{
    private Permission() { }

    [GeneratedRegex("^[A-Za-z]+\\.[A-Za-z]+$")]
    private static partial Regex PermissionName();
    
    public static Result<Permission> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Permission>.Failure("El nombre del permiso no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result<Permission>.Failure("El nombre del permiso no puede exceder los 30 caracteres");
        if (!PermissionName().IsMatch(name.Trim()))
            return Result<Permission>.Failure("El nombre del permiso es inválido");
        return Result<Permission>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim()
        });
    }
}