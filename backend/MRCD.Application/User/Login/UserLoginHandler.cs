using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Login;

internal sealed class UserLoginHandler(
    IUserRepository user,
    IRoleRepository role,
    IRolePermissionRepository rolePermission,
    IPermissionRepository permission
) : ICommandHandler<UserLoginCommand, IEnumerable<RoleWithPermissionDTO>>
{
    private readonly IUserRepository _user = user;
    private readonly IRoleRepository _role = role;
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly IPermissionRepository _permission = permission;

    public async Task<Result<IEnumerable<RoleWithPermissionDTO>>> HandleAsync(
        UserLoginCommand command,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return Result<IEnumerable<RoleWithPermissionDTO>>.Failure("El usuario es requerido");
        var user = await _user.GetByUsernameAsync(command.Username.Trim(), cancellationToken);
        if (user is null || !user.IsActive || !user.Password.Equals(command.Password))
            return Result<IEnumerable<RoleWithPermissionDTO>>.Failure("Credenciales inválidas");

        var rolesTask = _role.ByUserIdToListAsync(user.ID, cancellationToken);
        var rolePermissionTask = _rolePermission.ToListAsync(cancellationToken);
        var permissionTask = _permission.ToListAsync(cancellationToken);
        await Task.WhenAll(rolesTask, rolePermissionTask);

        var rolePermission = rolePermissionTask
            .Result
            .Select(rp => (rp.RoleID, rp.PermissionID))
            .ToHashSet();

        IEnumerable<PermissionUsedInRoleDTO> PermissionsByRole(Guid roleId) => permissionTask
            .Result
            .Select(p => new PermissionUsedInRoleDTO(
                p.ID,
                p.Name,
                rolePermission.Contains((roleId, p.ID))
            ));

        return Result<IEnumerable<RoleWithPermissionDTO>>.Success(rolesTask.Result.Select(r => new RoleWithPermissionDTO(
            r.ID,
            r.Name,
            PermissionsByRole(r.ID)
        )));
    }
}