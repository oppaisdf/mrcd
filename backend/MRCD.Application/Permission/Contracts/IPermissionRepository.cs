namespace MRCD.Application.Permission.Contracts;

public interface IPermissionRepository
{
    void Add(Domain.Role.Permission permission);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Domain.Role.Permission>> ToListAsync(CancellationToken cancellationToken);
}