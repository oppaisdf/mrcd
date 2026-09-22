using MRCD.Application.User.Contracts;

namespace MRCD.Application.Security;

internal sealed class PermissionCacheInvalidator(
    IUserRepository users,
    IPermissionCache cache
)
{
    public async Task InvalidateActiveUsersAsync(
        CancellationToken ct
    )
    {
        foreach (var user in (await users.ToListAsync(ct)).Where(u => u.IsActive))
            await cache.InvalidateAsync(user.ID, ct);
    }
}
