using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.DelActivity;

internal sealed class DelActivityHandler(
    IActivityRepository repo,
    ILogger<DelActivityHandler> logs
) : ICommandHandler<DelActivityCommand>
{
    private readonly IActivityRepository _repo = repo;
    private readonly ILogger<DelActivityHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelActivityCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.ExistsIdAsync(command.ActivityId, cancellationToken);
        if (!exists)
            return Result.Failure("La actividad no existe");
        await _repo.DeleteAsync(command.ActivityId, cancellationToken);
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("Activity {activity} has been deleted.", command.ActivityId);
        }
        return Result.Success();
    }
}