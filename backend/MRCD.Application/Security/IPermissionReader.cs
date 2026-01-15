namespace MRCD.Application.Security;

public interface IPermissionReader
{
    Task<PermissionSet> GetEffectivePermissionsAsync(Guid userId, CancellationToken cancellationToken);
}