using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Services.Common;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.Add;

internal sealed class AddRoleHandler(
    ICommonService service,
    IRoleRepository repo,
    IPersistenceContext save,
    AuditLog<AddRoleHandler> logs
) : ICommandHandler<AddRoleCommand, Guid>
{
    private readonly IRoleRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly AuditLog<AddRoleHandler> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(command.RoleName))
            return Result<Guid>.Failure("El nombre del rol no puede ser nulo");
        if (!_service.HasOnlyLetters(command.RoleName))
            return Result<Guid>.Failure("El nombre del rol solo puede contener letras");
        var alreadExists = await _repo.AlreadyExistsAsync(command.RoleName.Trim(), cancellationToken);
        if (alreadExists)
            return Result<Guid>.Failure("El nombre del rol ya está en uso");

        var role = Domain.Role.Role.Create(command.RoleName.Trim());
        if (!role.IsSuccess && role.Value is null) return Result<Guid>.Failure(role.Error!);
        _repo.Add(role.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.Write(command.UserId, "Role {role} with ID {id} has been created.", command.RoleName, role.Value!.ID);
    }
}