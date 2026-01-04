using Microsoft.EntityFrameworkCore;
using MRCD.Application.User.Contracts;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Repositories;

internal sealed class UserRepository(
    Persistence.AppContext app
) : IUserRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        User user
    ) => _app
        .Users
        .Add(user);

    public Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Users
        .SingleOrDefaultAsync(u => u.ID == id, cancellationToken);

    public Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken
    ) => _app
        .Users
        .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);

    public Task<bool> IsActiveAsync(
        Guid userId,
        CancellationToken cancellationToken
    ) => _app
        .Users
        .AnyAsync(u =>
            u.ID == userId
            && u.IsActive,
            cancellationToken
        );

    public Task<List<User>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Users
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}