using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.AddPermission;

internal sealed class AddPermissionHandler(
    ICommonService service,
    IPermissionRepository repo,
    IPersistenceContext save
) : ICommandHandler<AddPermissionCommand, Guid>
{
    private readonly IPermissionRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly ICommonService _service = service;

    public async Task<Result<Guid>> HandleAsync(
        AddPermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        if (!_service.IsValidPermission(command.PermissionName))
            return Result<Guid>.Failure("El nombre del permiso es inválido");
        var alreadyExists = await _repo.AlreadyExistsAsync(command.PermissionName.Trim(), cancellationToken);
        if (alreadyExists)
            return Result<Guid>.Failure("El nombre del permiso ya existe");
        var permission = Domain.Role.Permission.Create(command.PermissionName.Trim());
        if (!permission.IsSuccess)
            return Result<Guid>.Failure(permission.Error!);
        _repo.Add(permission.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(permission.Value!.ID);
    }
}