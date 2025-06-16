using api.Models.Entities;
using api.Models.Responses;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IPlannerRepository
{
    Task<ICollection<DayResponse>> ActivitiesInDaysToListAsync(ushort year, ushort month);
    Task<PlannerResponse?> GetByIdAsync(uint id);
    Task<IEnumerable<StageResponse>> StagesToListAsync();
    Task<uint> CreateActivityAsync(string userId, Activity activity);
    Task<List<string>> StageNamesToListAsync();
    Task CreateStageAsync(string userId, ActivityStage stage);
    Task<bool> ActivityAndStageExistsAsync(uint activityId, ushort stageId);
    Task AddStageToActivityAsync(StagesOfActivities stage);
    Task DeleteActivityAsync(string userId, uint id);
    Task<bool> UsingStageAsync(ushort id);
    Task DeleteStageAsync(string userId, ushort id);
    Task DelStageToActivityAsync(uint activityId, ushort stageId);
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
        var activityExists = await _context.Activities
            .AsNoTracking()
            .AnyAsync(a => a.Id == activityId)
            .ConfigureAwait(false);
        var stageExists = await _context.ActivityStages
            .AsNoTracking()
            .AnyAsync(s => s.Id == stageId)
            .ConfigureAwait(false);
        return activityExists && stageExists;
    }

    public async Task AddStageToActivityAsync(
        StagesOfActivities stage
    )
    {
        _context.StagesOfActivities.Add(stage);
        await _context.SaveChangesAsync().ConfigureAwait(false);
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
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _logs.RegisterCreationAsync(userId, $"Activity {activity.Id}").ConfigureAwait(false);
            await tran.CommitAsync().ConfigureAwait(false);
            return activity.Id!.Value;
        }
        catch
        {
            await tran.RollbackAsync().ConfigureAwait(false);
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
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _logs.RegisterCreationAsync(userId, $"Stage {stage.Id}").ConfigureAwait(false);
            await tran.CommitAsync().ConfigureAwait(false);
        }
        catch
        {
            await tran.RollbackAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task<ICollection<DayResponse>> ActivitiesInDaysToListAsync(
        ushort year,
        ushort month
    ) => await _context.Activities
            .AsNoTracking()
            .Where(a => a.Date.Year == year && a.Date.Month == month)
            .GroupBy(a => a.Date.Day)
            .Select(grp => new DayResponse(
                (ushort)grp.Key,
                grp.Select(g => new SimpleActivityResponse(
                    g.Id!.Value,
                    g.Name
                ))
            ))
            .ToListAsync()
            .ConfigureAwait(false);

    public async Task<PlannerResponse?> GetByIdAsync(
        uint id
    ) => await (
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

    public async Task<List<string>> StageNamesToListAsync()
    => await _context.ActivityStages
        .AsNoTracking()
        .Select(s => s.Name)
        .ToListAsync()
        .ConfigureAwait(false);

    public async Task DeleteActivityAsync(
        string userId,
        uint id
    )
    {
        var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.StagesOfActivities
                .Where(sa => sa.ActivityId == id)
                .ExecuteDeleteAsync()
                .ConfigureAwait(false);
            await _context.Activities
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync()
                .ConfigureAwait(false);
            await _logs
                .RegisterUpdateAsync(userId, $"Actividad removida {id}")
                .ConfigureAwait(false);
            await tran.CommitAsync().ConfigureAwait(false);
        }
        catch
        {
            await tran.RollbackAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task DeleteStageAsync(
        string userId,
        ushort id
    )
    {
        var tran = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            await _context.ActivityStages
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync()
                .ConfigureAwait(false);
            await _logs
                .RegisterUpdateAsync(userId, $"Eliminó fase de actividad {id}")
                .ConfigureAwait(false);
            await tran.CommitAsync().ConfigureAwait(false);
        }
        catch
        {
            await tran.RollbackAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task DelStageToActivityAsync(
        uint activityId,
        ushort stageId
    ) => await _context.StagesOfActivities
        .Where(sa => sa.ActivityId == activityId && sa.StageId == stageId)
        .ExecuteDeleteAsync()
        .ConfigureAwait(false);

    public async Task<bool> UsingStageAsync(ushort id)
    => await _context.StagesOfActivities
        .AsNoTracking()
        .AnyAsync(sa => sa.StageId == id)
        .ConfigureAwait(false);

    public async Task<IEnumerable<StageResponse>> StagesToListAsync()
        => await _context.ActivityStages
            .AsNoTracking()
            .Select(sa => new StageResponse(
                sa.Id!.Value,
                sa.Name
            ))
            .ToListAsync()
            .ConfigureAwait(false);
}