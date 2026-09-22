using MRCD.Application.Role.Contracts;
using MRCD.Application.User.Contracts;

namespace MRCD.Application.User.Services;

internal sealed class UserRegistration(
    IUserRepository users,
    IRoleRepository roles,
    IUserRoleRepository links
)
{
    public async Task<IReadOnlyList<Guid>> ResolveRolesAsync(
        IEnumerable<Guid> ids,
        CancellationToken ct
    )
    {
        var available = await roles.ToListAsync(ct);
        return [.. available
            .Where(r => r.Name != "sys")
            .Select(r => r.ID)
            .Intersect(ids)
        ];
    }

    public void Add(
        Domain.User.User user,
        IReadOnlyList<Guid> roleIds
    )
    {
        users.Add(user);
        links.AddRange(
            roleIds.Select(id => new Domain.User.UserRole(id, user.ID))
        );
    }
}
