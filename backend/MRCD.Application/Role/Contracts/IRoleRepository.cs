namespace MRCD.Application.Role.Contracts;

public interface IRoleRepository
{
    Task<bool> AlreadyExistsAsync(string name, CancellationToken cancellationToken);
    void Add(Domain.Role.Role role);
    Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Domain.Role.Role>> ToListAsync(CancellationToken cancellationToken);
}