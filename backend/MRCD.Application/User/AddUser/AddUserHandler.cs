using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.User.AddUser;

internal sealed class AddUserHandler(
    IUserRepository repo,
    IRoleRepository role,
    IUserRoleRepository userRole,
    IPersistenceContext save,
    ILogger<AddUserHandler> logs,
    ICommonService service
) : ICommandHandler<AddUserCommand, Guid>
{
    private readonly IUserRepository _repo = repo;
    private readonly IRoleRepository _role = role;
    private readonly IUserRoleRepository _userRole = userRole;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddUserHandler> _logs = logs;
    private readonly ICommonService _service = service;

    private async Task<IEnumerable<Guid>> ValidRoleIdsAsync(
        IEnumerable<Guid> rawRoles,
        CancellationToken ct
    )
    {
        var roles = await _role.ToListAsync(ct);
        var roleIds = roles
            .Where(r => !r.Name.Equals("sys"))
            .Select(r => r.ID);
        return roleIds
            .Intersect(rawRoles);
    }

    private async Task<bool> AlreadyExistsUsernameAsync(
        string username,
        CancellationToken ct
    )
    {
        var normalizedName = _service.NormalizeString(username);
        var users = await _repo.ToListAsync(ct);
        var normalizedNames = users
            .Select(u => _service.NormalizeString(u.Username));
        return normalizedNames.Contains(normalizedName);
    }

    public async Task<Result<Guid>> HandleAsync(
        AddUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var roles = await ValidRoleIdsAsync(command.Roles, cancellationToken);
        if (!roles.Any())
            return Result<Guid>.Failure("No se encontraron roles válidos");
        var user = Domain.User.User.Create(command.Username, command.Password);
        if (!user.IsSuccess)
            return Result<Guid>.Failure(user.Error!);
        var alreadyExists = await AlreadyExistsUsernameAsync(user.Value!.Username, cancellationToken);
        if (alreadyExists)
            return Result<Guid>.Failure("El usuario ya está en uso");
        _repo.Add(user.Value!);
        _userRole.AddRange(roles.Select(r => new Domain.User.UserRole(
            r,
            user.Value!.ID
        )));
        await _save.SaveChangesAsync(cancellationToken);
        _logs.LogInformation("User {user} has been added by user {creater}", user.Value!.ID, command.UserId);
        return Result<Guid>.Success(user.Value!.ID);
    }
}