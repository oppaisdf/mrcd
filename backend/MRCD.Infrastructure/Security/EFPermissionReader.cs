using Microsoft.EntityFrameworkCore;
using MRCD.Application.Security;

namespace MRCD.Infrastructure.Security;

internal sealed class EFPermmissionReader(
    Persistence.AppContext app
) : IPermissionReader
{
    public async Task<PermissionSet> GetEffectivePermissionsAsync(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var permissions = await (
            from ur in app.UserRoles
            join rp in app.RolesPermissions on ur.RoleID equals rp.RoleID
            join p in app.Permissions on rp.PermissionID equals p.ID
            where
                ur.UserID == userId
            select p.Name
        ).Distinct().ToListAsync(cancellationToken);
        return new(permissions.ToHashSet(StringComparer.OrdinalIgnoreCase));
    }
}