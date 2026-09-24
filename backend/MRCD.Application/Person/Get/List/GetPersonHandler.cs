using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;
using MRCD.Application.Services;

namespace MRCD.Application.Person.Get.List;

internal sealed class GetPersonHandler(
    AuditLog<GetPersonHandler> logs,
    IPersonRepository repo
) : IQueryHandler<Pagination<SimplePersonDTO>, GetPersonQuery>
{
    private readonly AuditLog<GetPersonHandler> _logs = logs;
    private readonly IPersonRepository _repo = repo;

    public async Task<Result<Pagination<SimplePersonDTO>>> HandleAsync(
        GetPersonQuery query,
        CancellationToken cancellationToken
    )
    {
        _logs.Write(query.UserId, "Paginated people has been consulted. Page {page}", query.Page);
        var normalized = string.IsNullOrWhiteSpace(query.Name)
            ? null
            : StringNormalizer.NormalizeString(query.Name);
        var results = query.Alert switch
        {
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