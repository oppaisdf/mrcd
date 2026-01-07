using Microsoft.EntityFrameworkCore;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Planner;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ActivityStageRepository(
    Persistence.AppContext app
) : IActivityStageRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        ActivityStage activityStage
    ) => _app
        .Add(activityStage);

    public Task<bool> AlreadyExistsAsync(
        Guid activityId,
        Guid stageId,
        Guid? userId,
        CancellationToken cancellationToken
    ) => _app
        .ActivitiesStages
        .AnyAsync(a =>
            a.ActivityId == activityId
            && a.StageId == stageId
            && a.UserId == userId,
            cancellationToken
        );

    public Task DeleteAsync(
        Guid activityId,
        Guid stageId,
        Guid? userId,
        CancellationToken cancellationToken
    ) => _app
        .ActivitiesStages
        .Where(a =>
            a.ActivityId == activityId
            && a.StageId == stageId
            && a.UserId == userId
        ).ExecuteDeleteAsync(cancellationToken);

    public Task<List<ActivityStage>> StagesByActivityToListAsync(
        Guid activityId,
        CancellationToken cancellationToken
    ) => _app
        .ActivitiesStages
        .Where(a => a.ActivityId == activityId)
        .ToListAsync(cancellationToken);
}