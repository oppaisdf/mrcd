using MRCD.Application.Services.Common;
using MRCD.Application.User.Contracts;

namespace MRCD.Application.User.Services;

internal sealed class UserNames(
    IUserRepository users,
    ICommonService normalizer
)
{
    public async Task<bool> ExistsAsync(
        string username,
        Guid? exceptId,
        CancellationToken ct
    )
    {
        var normalized = normalizer.NormalizeString(username);
        var existing = await users.ToListAsync(ct);
        return existing.Any(u =>
            u.ID != exceptId
            && normalizer.NormalizeString(u.Username) == normalized
        );
    }
}
