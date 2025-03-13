using api.Data;

namespace api.Services;

public interface IAlertService
{
    Task<ushort> CountAsync(ushort alert);
    Task<IEnumerable<object>> GetAsync(ushort alert);
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
            3 => await _repo.NoChildsCountAsync(),
            4 => await _repo.NoDocumentsCountAsync(),
            _ => 0,
        };
    }

    public async Task<IEnumerable<object>> GetAsync(
        ushort alert
    )
    {
        return alert switch
        {
            1 => await _repo.NoPaymentAsync(),
            2 => await _repo.NoGodparentsAsync(),
            3 => await _repo.NoChildsAsync(),
            4 => await _repo.NoDocumentsAsync(),
            _ => []
        };
    }
}