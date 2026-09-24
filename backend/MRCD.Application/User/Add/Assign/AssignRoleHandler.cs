using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.User;

namespace MRCD.Application.User.Add.Assign;

internal sealed class AssignRoleHandler(
    IUserRepository userRepo,
    IRoleRepository roleRepo,
    IUserRoleRepository userRole,
    IPersistenceContext save,
    IPermissionCache cache
) : ICommandHandler<AssignRoleCommand>
{
    public async Task<Result> HandleAsync(
        AssignRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepo.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null || !user.IsActive)
            return Result.Failure("El usuario no existe o está inactivo");
        var role = await roleRepo.GetByIdAsync(command.RoleId, cancellationToken);
        if (role is null || role.Name.Equals("sys"))
            return Result.Failure("El rol no existe");
        var userRoles = (await roleRepo.ByUserIdToListAsync(command.UserId, cancellationToken))
            .Select(r => r.ID);
        if (command.IsAssignment && userRoles.Contains(command.RoleId))
            return Result.Failure("El rol ya ha sido asignado");
        if (command.IsAssignment)
        {
            userRole.Add(new UserRole(
                command.RoleId,
                command.UserId
            ));
            await save.SaveChangesAsync(cancellationToken);
        }
        if (!command.IsAssignment && !userRoles.Contains(command.RoleId))
            return Result.Failure("El rol no ha sido asignado");
        if (!command.IsAssignment)
            await userRole.DeleteAsync(command.UserId, command.RoleId, cancellationToken);
        await cache.InvalidateAsync(command.UserId, cancellationToken);
        return Result.Success();
    }
}