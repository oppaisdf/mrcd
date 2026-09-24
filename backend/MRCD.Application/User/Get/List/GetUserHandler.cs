using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Get.List;

internal sealed class GetUserHandler(
    IUserRepository user,
    IRoleRepository role,
    IUserRoleRepository userRole
) : IQueryHandler<IEnumerable<UserDTO>>
{
    public async Task<Result<IEnumerable<UserDTO>>> HandleAsync(
        CancellationToken cancellationToken
    )
    {
        var users = await user.ToListAsync(cancellationToken);
        var roles = await role.ToListAsync(cancellationToken);
        var userRoles = await userRole.ToListAsync(cancellationToken);

        var assigned = userRoles
            .Select(ur => (ur.UserID, ur.RoleID))
            .ToHashSet();

        return Result<IEnumerable<UserDTO>>.Success(users.Select(u => new UserDTO(
            u.ID,
            u.Username,
            u.IsActive,
            roles
                .Select(r => new UsingRoleDTO(
                    r.ID,
                    r.Name,
                    assigned.Contains((u.ID, r.ID))
                ))
        )));
    }
}