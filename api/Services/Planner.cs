using api.Common;
using api.Data;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;

namespace api.Services;

public interface IPlannerService
{
    Task<ICollection<SimplePlannerResponse>> GetAsync(ushort year, ushort month);
    Task<PlannerResponse?> GetByIdAsync(uint id);
    Task<uint> CreateActivityAsync(string userId, ActivityRequest request);
    Task CreateStageAsync(string userId, StageRequest request);
    Task AddStageToActivityAsync(ActivityStageRequest request);
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
        var exists = await _repo.ActivityAndStageExistsAsync(request.ActivityId, request.StageId);
        if (!exists) throw new DoesNotExistsException("La actividad o la fase no existe");
        if (request.UserId != null && !await _user.UserActiveExists(request.UserId))
            throw new DoesNotExistsException("El usuario no existe");
        var stage = new StagesOfActivities
        {
            ActivityId = request.ActivityId,
            StageId = request.StageId,
            MainUser = request.MainUser,
            UserId = request.UserId,
            Notes = request.Notes
        };
        await _repo.AddStageToActivityAsync(stage);
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
        return await _repo.CreateActivityAsync(userId, activity);
    }

    public async Task CreateStageAsync(
        string userId,
        StageRequest request
    )
    {
        var stages = await _repo.StagesToListAsync();
        stages.ForEach(s => s = _service.GetNormalizedText(s));
        if (stages.Contains(request.Name)) throw new AlreadyExistsException("La etapa ya está registrada");
        var stage = new ActivityStage
        {
            Name = request.Name
        };
        await _repo.CreateStageAsync(userId, stage);
    }

    public async Task<ICollection<SimplePlannerResponse>> GetAsync(ushort year, ushort month)
        => await _repo.GetAsync(year, month);

    public async Task<PlannerResponse?> GetByIdAsync(uint id)
        => await _repo.GetByIdAsync(id);
}