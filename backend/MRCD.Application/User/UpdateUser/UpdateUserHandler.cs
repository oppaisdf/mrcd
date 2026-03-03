using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Services.CommonService;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.User.UpdateUser;

internal sealed class UpdateUserHandler(
    IUserRepository user,
    ICommonService service,
    IPersistenceContext save,
    ILogger<UpdateUserHandler> logs
) : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _user = user;
    private readonly ICommonService _service = service;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<UpdateUserHandler> _logs = logs;

    private async Task<bool> UniqueUsernameAsync(
        string username,
        CancellationToken ct
    )
    {
        var users = await _user.ToListAsync(ct);
        var normalizedNames = users
            .Select(u => _service.NormalizeString(u.Username));
        var normalizedName = _service.NormalizeString(username);
        return !normalizedNames.Contains(normalizedName);
    }

    public async Task<Result> HandleAsync(
        UpdateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        bool updated = false;

        var user = await _user.GetByIdAsync(command.Id, cancellationToken);
        if (user == null)
            return Result.Failure("El usuario no existe");

        if (!string.IsNullOrWhiteSpace(command.Username))
        {
            var uniqName = await UniqueUsernameAsync(command.Username.Trim(), cancellationToken);
            if (!uniqName)
                return Result.Failure("El usuario ya está en uso");
            var error = user.SetUsername(command.Username);
            if (!error.IsSuccess)
                return Result.Failure(error.Error!);
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(command.Password))
        {
            var error = user.SetPassword(command.Password);
            if (!error.IsSuccess)
                return Result.Failure(error.Error!);
            updated = true;
        }
        if (command.IsActive is not null)
        {
            var error = user.SetActive(command.IsActive.Value);
            if (!error.IsSuccess)
                return Result.Failure(error.Error!);
            updated = true;
        }
        if (!updated)
            return Result.Failure("No se encontraron datos a actualizar");
        await _save.SaveChangesAsync(cancellationToken);
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("User {user} with ID {id} has been updated.", user.Username, user.ID);
        }
        return Result.Success();
    }
}