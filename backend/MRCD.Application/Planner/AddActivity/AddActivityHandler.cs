using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.AddActivity;

internal sealed class AddActivityHandler(
    IActivityRepository repo,
    IPersistenceContext save,
    ILogger<AddActivityHandler> logs
) : ICommandHandler<AddActivityCommand, Guid>
{
    private readonly IActivityRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddActivityHandler> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddActivityCommand command,
        CancellationToken cancellationToken
    )
    {
        var activityResult = Domain.Planner.Activity.Create(command.ActivityName, command.Date);
        if (!activityResult.IsSuccess)
            return Result<Guid>.Failure(activityResult.Error!);
        _repo.Add(activityResult.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("Activity {activity} with ID {id} has been created.", command.ActivityName, activityResult.Value!.ID);
        }
        return Result<Guid>.Success(activityResult.Value!.ID);
    }
}