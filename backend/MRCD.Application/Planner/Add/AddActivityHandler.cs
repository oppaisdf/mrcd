using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.Add;

internal sealed class AddActivityHandler(
    IActivityRepository repo,
    IPersistenceContext save,
    AuditLog<AddActivityHandler> logs
) : ICommandHandler<AddActivityCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddActivityCommand command,
        CancellationToken cancellationToken
    )
    {
        var activityResult = Domain.Planner.Activity.Create(command.ActivityName, command.Date);
        if (!activityResult.IsSuccess)
            return Result<Guid>.Failure(activityResult.Error!);
        repo.Add(activityResult.Value!);
        await save.SaveChangesAsync(cancellationToken);
        logs.Write(command.UserId, "Activity {activity} with ID {id} has been created.", command.ActivityName, activityResult.Value!.ID);
        return Result<Guid>.Success(activityResult.Value!.ID);
    }
}