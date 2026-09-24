using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.Del;

internal sealed class DelPermissionHandler(
    IPermissionRepository repo,
    AuditLog<DelPermissionHandler> logs
) : ICommandHandler<DelPermissionCommand>
{
    public async Task<Result> HandleAsync(
        DelPermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await repo.IdExistsAsync(command.PermissionId, cancellationToken);
        if (!exists) return Result.Failure("El permiso no existe");
        await repo.DeleteAsync(command.PermissionId, cancellationToken);
        logs.Write(command.UserId, "Permission {permission} has been deleted.", command.PermissionId);
        return Result.Success();
    }
}