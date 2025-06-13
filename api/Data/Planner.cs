using api.Models.Entities;
using api.Models.Responses;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IPlannerRepository
{
    /// <summary>
    /// Retorna las actividades de un mes en un año específico
    /// </summary>
    /// <param name="Year">El año debe venir limpio</param>
    /// <param name="Month">El mes debe venir limpio</param>
    /// <returns></returns>
    Task<ICollection<SimplePlannerResponse>> GetAsync(ushort year, ushort month);
    Task<PlannerResponse?> GetByIdAsync(uint id);
    Task<uint> CreateActivityAsync(string userId, Activity activity);
    Task<List<string>> StagesToListAsync();
    Task CreateStageAsync(string userId, ActivityStage stage);
    Task<bool> ActivityAndStageExistsAsync(uint activityId, ushort stageId);
    Task AddStageToActivityAsync(StagesOfActivities stage);
}

public class PlannerRepository
(
    MerContext context,
    ILogService logs
) : IPlannerRepository
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;

    public async Task<bool> ActivityAndStageExistsAsync(
        uint activityId,
        ushort stageId
    )
    {
        var results = await Task.WhenAll(
            _context.Activities.AsNoTracking().AnyAsync(a => a.Id == activityId),
            _context.ActivityStages.AsNoTracking().AnyAsync(s => s.Id == stageId)
        ).ConfigureAwait(false);
        return results[0] && results[1];
    }

    public async Task AddStageToActivityAsync(
        StagesOfActivities stage
    )
    {
        _context.StagesOfActivities.Add(stage);
        await _context.SaveChangesAsync();
    }

    public async Task<uint> CreateActivityAsync(
        string userId,
        Activity activity
    )
    {
        var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            await _logs.RegisterCreationAsync(userId, $"Activity {activity.Id}");
            await tran.CommitAsync();
            return activity.Id!.Value;
        }
        catch
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task CreateStageAsync(
        string userId,
        ActivityStage stage
    )
    {
        var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.ActivityStages.Add(stage);
            await _context.SaveChangesAsync();
            await _logs.RegisterCreationAsync(userId, $"Stage {stage.Id}");
            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<ICollection<SimplePlannerResponse>> GetAsync(
        ushort year,
        ushort month
    )
    {
        return await _context.Activities
            .AsNoTracking()
            .Where(a => a.Date.Year == year && a.Date.Month == month)
            .Select(a => new SimplePlannerResponse(
                a.Id!.Value,
                a.Name,
                a.Date
            ))
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<PlannerResponse?> GetByIdAsync(
        uint id
    )
    {
        return await (
            from a in _context.Activities.AsNoTracking()
            where a.Id == id
            join sa in _context.StagesOfActivities on a.Id equals sa.ActivityId into g
            from sa in g.DefaultIfEmpty()
            join st in _context.ActivityStages on sa.StageId equals st.Id into h
            from st in h.DefaultIfEmpty()
            group new { sa, st } by new { a.Id, a.Name, a.Date } into grp
            select new PlannerResponse(
                grp.Key.Name,
                grp.Key.Date,
                grp.Where(x => x.sa != null)
                   .Select(x => new ActivityStageResponse(
                        x.st.Id!.Value,
                        x.st.Name,
                        x.sa.MainUser,
                        x.sa.UserId,
                        x.sa.Notes)
                    )
                   .ToList()
            ))
        .AsNoTracking()
        .FirstOrDefaultAsync()
        .ConfigureAwait(false);
    }

    public async Task<List<string>> StagesToListAsync()
    => await _context.ActivityStages
        .AsNoTracking()
        .Select(s => s.Name)
        .ToListAsync()
        .ConfigureAwait(false);
}