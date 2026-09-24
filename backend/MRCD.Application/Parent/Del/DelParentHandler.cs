using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.Del;

internal sealed class DelParentHandler(
    IParentRepository repo,
    AuditLog<DelParentHandler> logs
) : ICommandHandler<DelParentCommand>
{
    public async Task<Result> HandleAsync(
        DelParentCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await repo.ExistsAsync(command.ParentId, cancellationToken);
        if (!exists)
            return Result.Failure("El padre/padrino no existe");
        await repo.DeleteAsync(command.ParentId, cancellationToken);
        logs.Write(command.UserId, "Parent {parent} has been deleted.", command.ParentId);
        return Result.Success();
    }
}