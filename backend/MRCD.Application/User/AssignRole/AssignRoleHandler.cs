using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.User;

namespace MRCD.Application.User.AssignRole;

internal sealed class AssignRoleHandler(
    IUserRepository user,
    IRoleRepository role,
    IUserRoleRepository userRole,
    IPersistenceContext save,
    IPermissionCache cache
) : ICommandHandler<AssignRoleCommand>
{
    private readonly IUserRepository _user = user;
    private readonly IRoleRepository _role = role;
    private readonly IUserRoleRepository _userRole = userRole;
    private readonly IPersistenceContext _save = save;
    private readonly IPermissionCache _cache = cache;

    public async Task<Result> HandleAsync(
        AssignRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await _user.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null || !user.IsActive)
            return Result.Failure("El usuario no existe o está inactivo");
        var role = await _role.GetByIdAsync(command.RoleId, cancellationToken);
        if (role is null || role.Name.Equals("sys"))
            return Result.Failure("El rol no existe");
        var userRoles = (await _role.ByUserIdToListAsync(command.UserId, cancellationToken))
            .Select(r => r.ID);
        if (command.IsAssignment && userRoles.Contains(command.RoleId))
            return Result.Failure("El rol ya ha sido asignado");
        if (command.IsAssignment)
        {
            _userRole.Add(new UserRole(
                command.RoleId,
                command.UserId
            ));
            await _save.SaveChangesAsync(cancellationToken);
        }
        if (!command.IsAssignment && !userRoles.Contains(command.RoleId))
            return Result.Failure("El rol no ha sido asignado");
        if (!command.IsAssignment)
            await _userRole.DeleteAsync(command.UserId, command.RoleId, cancellationToken);
        await _cache.InvalidateAsync(command.UserId, cancellationToken);
        return Result.Success();
    }
}