namespace MRCD.Application.Role.Contracts;

public interface IRoleRepository
{
    Task<bool> AlreadyExistsAsync(string name, CancellationToken cancellationToken);
    void Add(Domain.Role.Role role);
    Task<List<Domain.Role.Role>> ByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> IdExistsAsync(Guid roleId, CancellationToken cancellationToken);
    Task<List<Domain.Role.Role>> ToListAsync(CancellationToken cancellationToken);
}