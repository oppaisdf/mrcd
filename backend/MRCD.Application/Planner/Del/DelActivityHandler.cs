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
    private readonly IActivityRepository _repo = repo;
    private readonly AuditLog<DelActivityHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelActivityCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.ExistsIdAsync(command.ActivityId, cancellationToken);
        if (!exists)
            return Result.Failure("La actividad no existe");
        await _repo.DeleteAsync(command.ActivityId, cancellationToken);
        _logs.Write(command.UserId, "Activity {activity} has been deleted.", command.ActivityId);
        return Result.Success();
    }
}