using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MRCD.Application.Security;

namespace MRCD.Infrastructure.Security;

internal sealed class DistributedPermissionCache(
    IDistributedCache cache
) : IPermissionCache
{
    private readonly IDistributedCache _cache = cache;
    private static string Key(Guid userId) => $"permissions:user:{userId:D}";
    private static readonly JsonSerializerOptions _jsonOptions = new(
        JsonSerializerDefaults.Web
    );

    public async Task<PermissionSet?> GetAsync(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var bytes = await _cache.GetAsync(Key(userId), cancellationToken);
        if (bytes is null) return null;

        var values = JsonSerializer.Deserialize<HashSet<string>>(bytes, _jsonOptions);
        return values is null
            ? null
            : new PermissionSet(values);
    }

    public Task InvalidateAsync(
        Guid userId,
        CancellationToken cancellationToken
    ) => _cache.RemoveAsync(Key(userId), cancellationToken);

    public async Task SetAsync(
        Guid userId,
        PermissionSet permissions,
        TimeSpan ttl,
        CancellationToken cancellationToken
    )
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(permissions.Values, _jsonOptions);

        await _cache.SetAsync(
            Key(userId),
            bytes,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            },
            cancellationToken
        );
    }
}