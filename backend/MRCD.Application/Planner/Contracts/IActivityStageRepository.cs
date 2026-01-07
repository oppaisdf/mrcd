using MRCD.Domain.Planner;

namespace MRCD.Application.Planner.Contracts;

public interface IActivityStageRepository
{
    void Add(ActivityStage activityStage);
    Task<bool> AlreadyExistsAsync(Guid activityId, Guid stageId, Guid? userId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid activityId, Guid stageId, Guid? userId, CancellationToken cancellationToken);
    Task<List<ActivityStage>> StagesByActivityToListAsync(Guid activityId, CancellationToken cancellationToken);
}