using Microsoft.EntityFrameworkCore;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Planner;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ActivityStageRepository(
    Persistence.AppContext app
) : IActivityStageRepository
{
    private readonly Persistence.AppContext _app = app;

    public Task<List<ActivityStage>> StagesByActivityToListAsync(
        Guid activityId,
        CancellationToken cancellationToken
    ) => _app
        .ActivitiesStages
        .Where(a => a.ActivityId == activityId)
        .ToListAsync(cancellationToken);
}