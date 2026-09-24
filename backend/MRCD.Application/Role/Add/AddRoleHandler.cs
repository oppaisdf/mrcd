using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.Add;

internal sealed class AddRoleHandler(
    IRoleRepository repo,
    IPersistenceContext save,
    AuditLog<AddRoleHandler> logs
) : ICommandHandler<AddRoleCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var role = Domain.Role.Role.Create(command.RoleName);
        if (!role.IsSuccess) return Result<Guid>.Failure(role.Error!);
        var alreadExists = await repo.AlreadyExistsAsync(command.RoleName.Trim(), cancellationToken);
        if (alreadExists)
            return Result<Guid>.Failure("El nombre del rol ya está en uso");

        repo.Add(role.Value!);
        await save.SaveChangesAsync(cancellationToken);
        logs.Write(command.UserId, "Role {role} with ID {id} has been created.", command.RoleName, role.Value!.ID);
        return Result<Guid>.Success(role.Value!.ID);
    }
}