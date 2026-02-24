namespace MRCD.Application.Abstracts;

public interface ICacheService
{
    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken
    ) where T : struct;
    Task SetAsync<T>(
        string key,
        T value,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null
    );
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}