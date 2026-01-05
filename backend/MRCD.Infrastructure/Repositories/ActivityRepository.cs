using Microsoft.EntityFrameworkCore;
using MRCD.Application.Planner.Contracts;
using MRCD.Domain.Planner;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ActivityRepository(
    Persistence.AppContext app
) : IActivityRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        Activity activity
    ) => _app
        .Activities
        .Add(activity);

    public Task DeleteAsync(
        Guid activityId,
        CancellationToken cancellationToken
    ) => _app
        .Activities
        .Where(a => a.ID == activityId)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<bool> ExistsIdAsync(
        Guid activityId,
        CancellationToken cancellationToken
    ) => _app
        .Activities
        .AnyAsync(a => a.ID == activityId, cancellationToken);

    public Task<Activity?> GetByIdAsync(
        Guid activityId,
        CancellationToken cancellationToken
    ) => _app
        .Activities
        .SingleOrDefaultAsync(a => a.ID == activityId, cancellationToken);

    public Task<List<Activity>> ToListAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken
    ) => _app
        .Activities
        .Where(a =>
            a.Date >= startDate
            && a.Date <= endDate
        ).ToListAsync(cancellationToken);
}