namespace MRCD.Application.Security;

public sealed class PermissionService(
    IPermissionCache cache,
    IPermissionReader reader
)
{
    private readonly IPermissionCache _cache = cache;
    private readonly IPermissionReader _reader = reader;

    public async Task<bool> HasPermissionAsync(
        Guid userId,
        string permission,
        CancellationToken cancellationToken
    )
    {
        var cached = await _cache.GetAsync(userId, cancellationToken);
        if (cached is not null)
            return cached.Contains(permission);
        var loaded = await _reader.GetEffectivePermissionsAsync(userId, cancellationToken);
        await _cache.SetAsync(userId, loaded, ttl: TimeSpan.FromMinutes(30), cancellationToken);
        return loaded.Contains(permission);
    }
}