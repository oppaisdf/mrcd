namespace MRCD.Application.Security;

public interface IPermissionCache
{
    Task<PermissionSet?> GetAsync(Guid userId, CancellationToken cancellationToken);
    Task SetAsync(Guid userId, PermissionSet permissions, TimeSpan ttl, CancellationToken cancellationToken);
    Task InvalidateAsync(Guid userId, CancellationToken cancellationToken);
}