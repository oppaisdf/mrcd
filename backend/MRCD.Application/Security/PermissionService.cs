namespace MRCD.Application.Security;

public sealed class PermissionService(
    IPermissionCache cache,
    IPermissionReader reader
)
{
    public async Task<bool> HasPermissionAsync(
        Guid userId,
        string permission,
        CancellationToken cancellationToken
    )
    {
        var cached = await cache.GetAsync(userId, cancellationToken);
        if (cached is not null)
            return cached.Contains(permission);
        var loaded = await reader.GetEffectivePermissionsAsync(userId, cancellationToken);
        await cache.SetAsync(userId, loaded, ttl: TimeSpan.FromMinutes(30), cancellationToken);
        return loaded.Contains(permission);
    }
}