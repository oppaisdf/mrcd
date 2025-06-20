using api.Models.Entities;
using api.Models.Responses;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IStageRepository
{
    Task<IEnumerable<StageResponse>> ToListAsync();
    Task<bool> UsingAsync(ushort id);
    Task DeleteAsync(string userId, ushort id);
    Task<List<string>> NamesToListAsync();
    Task CreateAsync(string userId, ActivityStage stage);
}

public class StageRepository(
    MerContext context,
    ILogService logs
) : IStageRepository
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;

    public async Task<IEnumerable<StageResponse>> ToListAsync()
    => await _context.ActivityStages
        .AsNoTracking()
        .Select(sa => new StageResponse(
            sa.Id!.Value,
            sa.Name
        ))
        .ToListAsync()
        .ConfigureAwait(false);

    public async Task<bool> UsingAsync(ushort id)
    => await _context.StagesOfActivities
        .AsNoTracking()
        .AnyAsync(sa => sa.StageId == id)
        .ConfigureAwait(false);

    public async Task DeleteAsync(
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

    public async Task<List<string>> NamesToListAsync()
    => await _context.ActivityStages
        .AsNoTracking()
        .Select(s => s.Name)
        .ToListAsync()
        .ConfigureAwait(false);

    public async Task CreateAsync(
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
}