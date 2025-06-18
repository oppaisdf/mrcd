using api.Common;
using api.Data;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;

namespace api.Services;

public interface IPlannerService
{
    Task<ICollection<DayResponse>> GetAsync(ushort year, ushort month);
    Task<PlannerResponse?> GetByIdAsync(uint id);
    Task<IEnumerable<StageResponse>> StagesToListAsync();
    Task<uint> CreateActivityAsync(string userId, ActivityRequest request);
    Task CreateStageAsync(string userId, StageRequest request);
    Task AddStageToActivityAsync(ActivityStageRequest request);
    Task DeleteActivityAsync(string userId, uint id);
    Task DeleteStageAsync(string userId, ushort id);
    Task DelStageToActivityAsync(uint activityId, ushort stageId);
}

public class PlannerService(
    IPlannerRepository repo,
    ICommonService service,
    IUserService user
) : IPlannerService
{
    private readonly IPlannerRepository _repo = repo;
    private readonly ICommonService _service = service;
    private readonly IUserService _user = user;

    public async Task AddStageToActivityAsync(
        ActivityStageRequest request
    )
    {
        var exists = await _repo
            .ActivityAndStageExistsAsync(request.ActivityId, request.StageId)
            .ConfigureAwait(false);
        if (!exists) throw new DoesNotExistsException("La actividad o la fase no existe");
        if (request.UserId != null && !await _user.UserActiveExists(request.UserId).ConfigureAwait(false))
            throw new DoesNotExistsException("El usuario no existe");
        if (await _repo.StageAlreadyAddedToActivity(request.ActivityId, request.StageId))
            throw new AlreadyExistsException("La fase ya fue agregada a la actividad :0");
        var stage = new StagesOfActivities
        {
            ActivityId = request.ActivityId,
            StageId = request.StageId,
            MainUser = request.MainUser,
            UserId = request.UserId,
            Notes = request.Notes
        };
        await _repo
            .AddStageToActivityAsync(stage)
            .ConfigureAwait(false);
    }

    public async Task<uint> CreateActivityAsync(
        string userId,
        ActivityRequest request
    )
    {
        var activity = new Activity
        {
            Name = request.Name,
            Date = request.Date
        };
        return await _repo
            .CreateActivityAsync(userId, activity)
            .ConfigureAwait(false);
    }

    public async Task CreateStageAsync(
        string userId,
        StageRequest request
    )
    {
        var stages = await _repo
            .StageNamesToListAsync()
            .ConfigureAwait(false);
        stages.ForEach(s => s = _service.GetNormalizedText(s));
        if (stages.Contains(request.Name)) throw new AlreadyExistsException("La etapa ya está registrada");
        var stage = new ActivityStage
        {
            Name = request.Name
        };
        await _repo
            .CreateStageAsync(userId, stage)
            .ConfigureAwait(false);
    }

    public async Task DeleteActivityAsync(
        string userId,
        uint id
    ) => await _repo.DeleteActivityAsync(userId, id);

    public async Task DeleteStageAsync(
        string userId,
        ushort id
    )
    {
        var usingStage = await _repo
            .UsingStageAsync(id)
            .ConfigureAwait(false);
        if (usingStage) throw new BadRequestException("No se puede eliminar porque la fase de actividad está en uso");
        await _repo
            .DeleteStageAsync(userId, id)
            .ConfigureAwait(false);
    }

    public async Task DelStageToActivityAsync(
        uint activityId,
        ushort stageId
    )
    {
        var exists = await _repo
            .ActivityAndStageExistsAsync(activityId, stageId)
            .ConfigureAwait(false);
        if (!exists) throw new DoesNotExistsException("La actividad o la fase no existe");
        await _repo
            .DelStageToActivityAsync(activityId, stageId)
            .ConfigureAwait(false);
    }

    public async Task<ICollection<DayResponse>> GetAsync(
        ushort year,
        ushort month
    )
    {
        var days = new List<DayResponse>();
        var lastDayInMonth = DateTime.DaysInMonth(year, month);
        var firstDayOfWeek = (short)new DateTime(year, month, 1).DayOfWeek;
        var bussinesDays = await _repo
            .ActivitiesInDaysToListAsync(year, month)
            .ConfigureAwait(false);
        var dirDays = bussinesDays.ToDictionary(d => d.Day, d => d);

        //Días en blanco
        for (short day = 0; day < firstDayOfWeek; day++)
            days.Add(new DayResponse(0, []));

        //Días reales del mes
        for (ushort day = 1; day <= lastDayInMonth; day++)
            days.Add(
                dirDays.TryGetValue(day, out DayResponse? value) ? value :
                new DayResponse(day, [])
            );

        // Completar la última fila con días vacíos hasta el final de la semana
        var remainingDaysInWeek = 7 - (days.Count % 7);
        if (remainingDaysInWeek < 7)
            for (short day = 0; day < remainingDaysInWeek; day++)
                days.Add(new DayResponse(0, []));

        return days;
    }

    public async Task<PlannerResponse?> GetByIdAsync(uint id)
    {
        var activity = await _repo
            .GetByIdAsync(id)
            .ConfigureAwait(false);
        if (activity == null) return null;
        var stages = await StagesToListAsync()
            .ConfigureAwait(false);
        var users = await _user
            .OnlyUserToListAsync()
            .ConfigureAwait(false);
        return new PlannerResponse(
            activity,
            stages,
            users
        );
    }

    public async Task<IEnumerable<StageResponse>> StagesToListAsync()
    => await _repo
        .StagesToListAsync()
        .ConfigureAwait(false);
}
