using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.GetPerson;

internal sealed class GetPersonHandler(
    ILogger<GetPersonHandler> logs,
    IPersonRepository repo,
    ICommonService service
) : IQueryHandler<Pagination<SimplePersonDTO>, GetPersonQuery>
{
    private readonly ILogger<GetPersonHandler> _logs = logs;
    private readonly IPersonRepository _repo = repo;
    private readonly ICommonService _service = service;

    public async Task<Result<Pagination<SimplePersonDTO>>> HandleAsync(
        GetPersonQuery query,
        CancellationToken cancellationToken
    )
    {
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = query.UserId
        }))
        {
            _logs.LogInformation("Paginated people has been consulted. Page {page}", query.Page);
        }
        var normalized = string.IsNullOrWhiteSpace(query.Name)
            ? null
            : _service.NormalizeString(query.Name);
        var results = query.Alert switch{
            Alert.Common.AlertType.PendingCharges => await _repo.PendingChargesToListAsync(
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            ),
            Alert.Common.AlertType.PendingDocuments => await _repo.PendingDocumentsToListAsync(
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            ),
            Alert.Common.AlertType.WithoutGodparents => await _repo.PendingGodparentsToListAsync(
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            ),
            _ => await _repo.ToListAsync(
                query.IsActive,
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            )
        };
        return Result<Pagination<SimplePersonDTO>>.Success(results);
    }
}