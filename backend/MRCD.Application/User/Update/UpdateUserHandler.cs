using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Logs;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Update;

internal sealed class UpdateUserHandler(
    IUserRepository users,
    UserNames names,
    IPersistenceContext save,
    AuditLog<UpdateUserHandler> audit
) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(command.Id, cancellationToken);
        if (user is null) return Result.Failure("El usuario no existe");
        if (!string.IsNullOrWhiteSpace(command.Username)
            && await names.ExistsAsync(command.Username, user.ID, cancellationToken)
        ) return Result.Failure("El usuario ya está en uso");
        
        var result = user.Update(command.Username, command.Password, command.IsActive);
        if (!result.IsSuccess) return result;
        await save.SaveChangesAsync(cancellationToken);
        audit.Write(command.UserId, "User {user} with ID {id} has been updated.", user.Username, user.ID);
        return Result.Success();
    }
}
