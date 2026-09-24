using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.Del;

internal sealed class DelActivityHandler(
    IActivityRepository repo,
    AuditLog<DelActivityHandler> logs
) : ICommandHandler<DelActivityCommand>
{
    public async Task<Result> HandleAsync(
        DelActivityCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await repo.ExistsIdAsync(command.ActivityId, cancellationToken);
        if (!exists)
            return Result.Failure("La actividad no existe");
        await repo.DeleteAsync(command.ActivityId, cancellationToken);
        logs.Write(command.UserId, "Activity {activity} has been deleted.", command.ActivityId);
        return Result.Success();
    }
}