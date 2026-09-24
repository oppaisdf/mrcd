using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.Add;

internal sealed class AddPermissionHandler(
    IPermissionRepository repo,
    IPersistenceContext save
) : ICommandHandler<AddPermissionCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddPermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        var permission = Domain.Role.Permission.Create(command.PermissionName);
        if (!permission.IsSuccess) return Result<Guid>.Failure(permission.Error!);
        var alreadyExists = await repo.AlreadyExistsAsync(command.PermissionName.Trim(), cancellationToken);
        if (alreadyExists)
            return Result<Guid>.Failure("El nombre del permiso ya existe");
        repo.Add(permission.Value!);
        await save.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(permission.Value!.ID);
    }
}