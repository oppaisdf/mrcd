using api.Data;
using api.Models.Responses;

namespace api.Services;

public interface IPlannerService
{
    Task<ICollection<SimplePlannerResponse>> GetAsync(ushort year, ushort month);
    Task<PlannerResponse?> GetByIdAsync(uint id);
}

public class PlannerService(
    IPlannerRepository repo
) : IPlannerService
{
    private readonly IPlannerRepository _repo = repo;

    public async Task<ICollection<SimplePlannerResponse>> GetAsync(ushort year, ushort month)
        => await _repo.GetAsync(year, month);

    public async Task<PlannerResponse?> GetByIdAsync(uint id)
        => await _repo.GetByIdAsync(id);
}