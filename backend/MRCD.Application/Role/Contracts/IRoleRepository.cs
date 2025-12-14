namespace MRCD.Application.Role.Contracts;

public interface IRoleRepository
{
    Task<bool> AlreadyExistsAsync(string name, CancellationToken cancellationToken);
    void Add(Domain.Role.Role role);
    Task<List<Domain.Role.RolePermission>> RolePermissionToListAsync(CancellationToken cancellationToken);
    Task<List<Domain.Role.Role>> ToListAsync(CancellationToken cancellationToken);
}