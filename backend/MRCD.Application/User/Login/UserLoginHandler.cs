using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Role.Contracts;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Login;

internal sealed class UserLoginHandler(
    IUserRepository user,
    IRoleRepository role,
    IRolePermissionRepository rolePermission,
    IPermissionRepository permission
) : ICommandHandler<UserLoginCommand, LoginDTO>
{
    private readonly IUserRepository _user = user;
    private readonly IRoleRepository _role = role;
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly IPermissionRepository _permission = permission;

    public async Task<Result<LoginDTO>> HandleAsync(
        UserLoginCommand command,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return Result<LoginDTO>.Failure("El usuario es requerido");
        var user = await _user.GetByUsernameAsync(command.Username.Trim(), cancellationToken);
        if (user is null || !user.IsActive || !user.Password.Equals(command.Password))
            return Result<LoginDTO>.Failure("Credenciales inválidas");

        var rolesTask = _role.ByUserIdToListAsync(user.ID, cancellationToken);
        var rolePermissionTask = _rolePermission.ToListAsync(cancellationToken);
        var permissionTask = _permission.ToListAsync(cancellationToken);
        await Task.WhenAll(rolesTask, rolePermissionTask);

        var rolePermissions = (
            from r in rolesTask.Result
            join rp in rolePermissionTask.Result on r.ID equals rp.RoleID
            join p in permissionTask.Result on rp.PermissionID equals p.ID
            group p by r into R
            select new
            {
                RoleName = R.Key.Name,
                Permissions = R.Select(x => x.Name)
            }
        ).ToList();
        return Result<LoginDTO>.Success(new(
            user.ID,
            rolePermissions
                .Select(rp => rp.RoleName)
                .Distinct(),
            rolePermissions
                .SelectMany(rp => rp.Permissions.Select(p => p))
                .Distinct()
        ));
    }
}