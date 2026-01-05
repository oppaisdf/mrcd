namespace MRCD.Application.Planner.Contracts;

public interface IActivityRepository
{
    void Add(Domain.Planner.Activity activity);
    Task DeleteAsync(Guid activityId, CancellationToken cancellationToken);
    Task<bool> ExistsIdAsync(Guid activityId, CancellationToken cancellationToken);
    Task<Domain.Planner.Activity?> GetByIdAsync(Guid activityId, CancellationToken cancellationToken);
    Task<List<Domain.Planner.Activity>> ToListAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);
}