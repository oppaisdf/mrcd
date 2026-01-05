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
        _logs.LogInformation("Activity {activity} has been created by user {user}", activityResult.Value!.ID, command.UserId);
        return Result<Guid>.Success(activityResult.Value!.ID);
    }
}