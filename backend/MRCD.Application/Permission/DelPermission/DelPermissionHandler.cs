using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.DelPermission;

internal sealed class DelPermissionHandler(
    IPermissionRepository repo,
    ILogger<DelPermissionHandler> logs
) : ICommandHandler<DelPermissionCommand>
{
    private readonly IPermissionRepository _repo = repo;
    private readonly ILogger<DelPermissionHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelPermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.IdExistsAsync(command.PermissionId, cancellationToken);
        if (!exists) return Result.Failure("El permiso no existe");
        await _repo.DeleteAsync(command.PermissionId, cancellationToken);
        _logs.LogInformation("Permission {id} has been deleted by user {user}", command.PermissionId, command.UserId);
        return Result.Success();
    }
}