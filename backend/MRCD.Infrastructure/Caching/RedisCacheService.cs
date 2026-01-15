using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MRCD.Application.Abstracts;

namespace MRCD.Infrastructure.Caching;

internal sealed class RedisCacheService(
    IDistributedCache cache
) : ICacheService
{
    private readonly IDistributedCache _cache = cache;
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken
    )
    {
        var bytes = await _cache.GetAsync(key, cancellationToken);
        if (bytes is null || bytes.Length == 0) return default;
        return JsonSerializer.Deserialize<T>(bytes, _serializerOptions);
    }

    public Task RemoveAsync(
        string key,
        CancellationToken cancellationToken
    ) => _cache.RemoveAsync(key, cancellationToken);

    public async Task SetAsync<T>(
        string key,
        T value,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null
    )
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _serializerOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromMinutes(5)
        };
        await _cache.SetAsync(key, bytes, options, cancellationToken);
    }
}