using api.Data;
using api.Models.Responses;

namespace api.Services;

public interface IAlertService
{
    Task<ushort> CountAsync(ushort alert);
    Task<IEnumerable<PersonResponse>> GetAsync(ushort alert);
}

public class AlertService(
    IAlertRepository repo
) : IAlertService
{
    private readonly IAlertRepository _repo = repo;

    public async Task<ushort> CountAsync(
        ushort alert
    )
    {
        return alert switch
        {
            1 => await _repo.NoPaymentCountAsync(),
            2 => await _repo.NoGodparentsCountAsync(),
            _ => 0,
        };
    }

    public async Task<IEnumerable<PersonResponse>> GetAsync(
        ushort alert
    )
    {
        return alert switch
        {
            1 => await _repo.NoPaymentAsync(),
            2 => await _repo.NoGodparentsAsync(),
            _ => []
        };
    }
}