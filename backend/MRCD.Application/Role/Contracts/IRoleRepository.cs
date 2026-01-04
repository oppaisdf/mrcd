namespace MRCD.Application.Role.Contracts;

public interface IRoleRepository
{
    Task<bool> AlreadyExistsAsync(string name, CancellationToken cancellationToken);
    void Add(Domain.Role.Role role);
    Task<Domain.Role.Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task<List<Domain.Role.Role>> ByUserIdToListAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> IdExistsAsync(Guid roleId, CancellationToken cancellationToken);
    Task<List<Domain.Role.Role>> ToListAsync(CancellationToken cancellationToken);
}