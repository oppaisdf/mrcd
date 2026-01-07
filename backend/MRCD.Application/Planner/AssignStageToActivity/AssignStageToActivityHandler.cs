using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Planner.Contracts;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.Planner;

namespace MRCD.Application.Planner.AssignStageToActivity;

internal sealed class AssignStageToActivityHandler(
    IBaseEntityRepository<Stage> stage,
    IActivityRepository activity,
    IActivityStageRepository activityStage,
    IUserRepository user,
    IPersistenceContext save
) : ICommandHandler<AssignStageToActivityCommand>
{
    private readonly IBaseEntityRepository<Stage> _stage = stage;
    private readonly IActivityRepository _activity = activity;
    private readonly IActivityStageRepository _activityStage = activityStage;
    private readonly IUserRepository _user = user;
    private readonly IPersistenceContext _save = save;

    private async Task<Result> DeleteAsync(
        AssignStageToActivityCommand command,
        CancellationToken ct
    )
    {
        var exists = await _activityStage.AlreadyExistsAsync(
            command.ActivityId,
            command.StageId,
            command.UserId,
            ct
        );
        if (!exists)
            return Result.Failure("La fase no ha sido asignada a la actividad");
        await _activityStage.DeleteAsync(command.ActivityId, command.StageId, command.UserId, ct);
        return Result.Success();
    }

    private async Task<Result> AddAsync(
        AssignStageToActivityCommand command,
        CancellationToken ct
    )
    {
        var alreadyExists = await _activityStage.AlreadyExistsAsync(
            command.ActivityId,
            command.StageId,
            command.UserId,
            ct
        );
        if (alreadyExists)
            return Result.Failure("La fase ya ha sido agregada a la actividad");
        var notes = string.IsNullOrWhiteSpace(command.Notes)
            ? null : command.Notes.Trim();
        if (notes is not null && notes.Length > 50)
            return Result.Failure("Las nota no puede exceder los 50 caracteres");

        var stageTask = _stage.ExistsIdAsync(command.StageId, ct);
        var activityTask = _activity.ExistsIdAsync(command.ActivityId, ct);
        await Task.WhenAll(stageTask, activityTask);
        if (!activityTask.Result)
            return Result.Failure("La actividad no existe");
        if (!stageTask.Result)
            return Result.Failure("La fase de actividad no existe");
        if (command.UserId is not null)
        {
            var userExistsActive = await _user.IsActiveAsync(command.UserId.Value, ct);
            if (!userExistsActive)
                return Result.Failure("El usuario no existe o está inactivo");
        }

        _activityStage.Add(new(
            command.ActivityId,
            command.StageId,
            command.IsUserMain,
            command.UserId,
            notes
        ));
        await _save.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> HandleAsync(
        AssignStageToActivityCommand command,
        CancellationToken cancellationToken
    ) => command.IsAssignation
        ? await AddAsync(command, cancellationToken)
        : await DeleteAsync(command, cancellationToken);
}