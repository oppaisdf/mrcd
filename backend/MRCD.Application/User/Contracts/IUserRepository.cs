namespace MRCD.Application.User.Contracts;

public interface IUserRepository
{
    void Add(Domain.User.User user);
    Task<Domain.User.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Domain.User.User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<List<Domain.User.User>> ToListAsync(CancellationToken cancellationToken);
}