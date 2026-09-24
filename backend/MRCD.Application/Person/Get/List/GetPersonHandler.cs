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
    public async Task<Result<Pagination<SimplePersonDTO>>> HandleAsync(
        GetPersonQuery query,
        CancellationToken cancellationToken
    )
    {
        logs.Write(query.UserId, "Paginated people has been consulted. Page {page}", query.Page);
        var normalized = string.IsNullOrWhiteSpace(query.Name)
            ? null
            : StringNormalizer.NormalizeString(query.Name);
        var results = query.Alert switch
        {
            Alert.Common.AlertType.PendingCharges => await repo.PendingChargesToListAsync(
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            ),
            Alert.Common.AlertType.PendingDocuments => await repo.PendingDocumentsToListAsync(
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            ),
            Alert.Common.AlertType.WithoutGodparents => await repo.PendingGodparentsToListAsync(
                query.Page,
                20,
                normalized,
                query.IsSunday,
                query.IsMasculine,
                cancellationToken
            ),
            _ => await repo.ToListAsync(
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