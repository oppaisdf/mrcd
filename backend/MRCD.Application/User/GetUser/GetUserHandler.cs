using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.User.GetUser;

internal sealed class GetUserHandler(
    IUserRepository user,
    IRoleRepository role,
    IUserRoleRepository userRole
) : IQueryHandler<IEnumerable<UserDTO>>
{
    private readonly IUserRepository _user = user;
    private readonly IRoleRepository _role = role;
    private readonly IUserRoleRepository _userRole = userRole;

    public async Task<Result<IEnumerable<UserDTO>>> HandleAsync(
        CancellationToken cancellationToken
    )
    {
        var userTask = _user.ToListAsync(cancellationToken);
        var roleTask = _role.ToListAsync(cancellationToken);
        var userRoleTask = _userRole.ToListAsync(cancellationToken);
        await Task.WhenAll(userTask, roleTask, userRoleTask);

        var assigned = userRoleTask
            .Result
            .Select(ur => (ur.UserID, ur.RoleID))
            .ToHashSet();

        return Result<IEnumerable<UserDTO>>.Success(userTask.Result.Select(u => new UserDTO(
            u.ID,
            u.Username,
            u.IsActive,
            roleTask
                .Result
                .Select(r => new UsingRoleDTO(
                    r.ID,
                    r.Name,
                    assigned.Contains((u.ID, r.ID))
                ))
        )));
    }
}