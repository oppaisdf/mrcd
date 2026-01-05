using MRCD.Domain.Planner;

namespace MRCD.Application.Planner.Contracts;

public interface IActivityStageRepository
{
    Task<List<ActivityStage>> StagesByActivityToListAsync(Guid activityId, CancellationToken cancellationToken);
}