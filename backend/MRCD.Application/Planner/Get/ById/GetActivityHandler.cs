using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Planner.Contracts;
using MRCD.Application.Planner.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.Get.ById;

internal sealed class GetActivityHandler(
    IActivityRepository activity,
    IBaseEntityRepository<Domain.Planner.Stage> stage,
    IActivityStageRepository activityStage,
    IUserRepository user
) : IQueryHandler<ActivityDTO, GetActivityQuery>
{
    private readonly IActivityRepository _activity = activity;
    private readonly IBaseEntityRepository<Domain.Planner.Stage> _stage = stage;
    private readonly IActivityStageRepository _activityStage = activityStage;
    private readonly IUserRepository _user = user;

    public async Task<Result<ActivityDTO>> HandleAsync(
        GetActivityQuery query,
        CancellationToken cancellationToken
    )
    {
        var activity = await _activity.GetByIdAsync(query.ActivityId, cancellationToken);
        if (activity is null)
            return Result<ActivityDTO>.Failure("La actividad no existe");
        var availableStages = await _stage.ToListAsync(cancellationToken);
        var assignedStages = await _activityStage.StagesByActivityToListAsync(query.ActivityId, cancellationToken);
        var users = await _user.ToListAsync(cancellationToken);

        var stages =
            from s in availableStages
            join a in assignedStages on s.ID equals a.StageId
            join u in users on a.UserId equals u.ID into tempU
            from u in tempU.DefaultIfEmpty()
            select new StageDTO(
                s.ID,
                s.Name,
                a.IsUserMain,
                a.UserId,
                a.UserId is null
                    ? null
                    : u?.Username,
                a.Notes
            );
        return Result<ActivityDTO>.Success(new ActivityDTO(
            activity.ID,
            activity.Name,
            activity.Date,
            stages
        ));
    }
}