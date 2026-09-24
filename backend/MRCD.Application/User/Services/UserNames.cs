using MRCD.Application.Services;
using MRCD.Application.User.Contracts;

namespace MRCD.Application.User.Services;

internal sealed class UserNames(
    IUserRepository users
)
{
    public async Task<bool> ExistsAsync(
        string username,
        Guid? exceptId,
        CancellationToken ct
    )
    {
        var normalized = StringNormalizer.NormalizeString(username);
        var existing = await users.ToListAsync(ct);
        return existing.Any(u =>
            u.ID != exceptId
            && StringNormalizer.NormalizeString(u.Username) == normalized
        );
    }
}
