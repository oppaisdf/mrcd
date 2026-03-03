using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.AddRole;

internal sealed class AddRoleHandler(
    IRoleRepository repo,
    IPersistenceContext save,
    ILogger<AddRoleHandler> logs
) : ICommandHandler<AddRoleCommand, Guid>
{
    private readonly IRoleRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddRoleHandler> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(command.RoleName))
            return Result<Guid>.Failure("El nombre del rol no puede ser nulo");
        var alreadExists = await _repo.AlreadyExistsAsync(command.RoleName.Trim(), cancellationToken);
        if (alreadExists)
            return Result<Guid>.Failure("El nombre del rol ya está en uso");

        var role = Domain.Role.Role.Create(command.RoleName.Trim());
        if (!role.IsSuccess && role.Value is null) return Result<Guid>.Failure(role.Error!);
        _repo.Add(role.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("Role {role} with ID {id} has been created.", command.RoleName, role.Value!.ID);
        }
        return Result<Guid>.Success(role.Value.ID);
    }
}