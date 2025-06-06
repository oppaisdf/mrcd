using api.Models.Responses;
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
}

public class PlannerRepository
(
    MerContext context
) : IPlannerRepository
{
    private readonly MerContext _context = context;

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
        .FirstOrDefaultAsync()
        .ConfigureAwait(false);
    }
}