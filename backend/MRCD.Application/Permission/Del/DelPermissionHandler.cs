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
    private readonly IPermissionRepository _repo = repo;
    private readonly AuditLog<DelPermissionHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelPermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.IdExistsAsync(command.PermissionId, cancellationToken);
        if (!exists) return Result.Failure("El permiso no existe");
        await _repo.DeleteAsync(command.PermissionId, cancellationToken);
        _logs.Write(command.UserId, "Permission {permission} has been deleted.", command.PermissionId);
        return Result.Success();
    }
}