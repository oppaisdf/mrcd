using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Planner.Contracts;
using MRCD.Application.Planner.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.Get.ById;

internal sealed class GetActivityHandler(
    IActivityRepository repo,
    IBaseEntityRepository<Domain.Planner.Stage> stage,
    IActivityStageRepository activityStage,
    IUserRepository user
) : IQueryHandler<ActivityDTO, GetActivityQuery>
{
    public async Task<Result<ActivityDTO>> HandleAsync(
        GetActivityQuery query,
        CancellationToken cancellationToken
    )
    {
        var activity = await repo.GetByIdAsync(query.ActivityId, cancellationToken);
        if (activity is null)
            return Result<ActivityDTO>.Failure("La actividad no existe");
        var availableStages = await stage.ToListAsync(cancellationToken);
        var assignedStages = await activityStage.StagesByActivityToListAsync(query.ActivityId, cancellationToken);
        var users = await user.ToListAsync(cancellationToken);

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