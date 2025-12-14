namespace MRCD.Application.Permission.Contracts;

public interface IPermissionRepository
{
    void Add(Domain.Role.Permission permission);
    Task<bool> AlreadyExistsAsync(string name, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Domain.Role.Permission>> ToListAsync(CancellationToken cancellationToken);
}