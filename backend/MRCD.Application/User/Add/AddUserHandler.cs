using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Logs;
using MRCD.Application.User.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Add;

internal sealed class AddUserHandler(
    UserRegistration registration,
    UserNames names,
    IPersistenceContext save,
    AuditLog<AddUserHandler> audit
) : ICommandHandler<AddUserCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddUserCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.Roles is null) return Result<Guid>.Failure("La lista de roles es requerida");
        var roles = await registration.ResolveRolesAsync(command.Roles, cancellationToken);
        if (roles.Count == 0) return Result<Guid>.Failure("No se encontraron roles válidos");
        var created = Domain.User.User.Create(command.Username, command.Password);
        if (!created.IsSuccess) return Result<Guid>.Failure(created.Error!);
        var user = created.Value!;
        if (await names.ExistsAsync(
            user.Username,
            exceptId: null,
            cancellationToken)
        ) return Result<Guid>.Failure("El usuario ya está en uso");
        registration.Add(user, roles);
        await save.SaveChangesAsync(cancellationToken);
        audit.Write(command.UserId, "User {user} with ID {id} has been created.", user.Username, user.ID);
        return Result<Guid>.Success(user.ID);
    }
}
