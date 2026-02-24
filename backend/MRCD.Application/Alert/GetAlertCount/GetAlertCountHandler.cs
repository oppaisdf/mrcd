using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Alert.Contracts;
using MRCD.Application.Alert.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Alert.GetAlertCount;

internal sealed class GetAlertCountHandler(
    IAlertRepository repo,
    ICacheService cache
) : IQueryHandler<AlertDTO, GetAlertCountQuery>
{
    private readonly IAlertRepository _repo = repo;
    private readonly ICacheService _cache = cache;

    private async Task<int> QueryAsync(
        AlertType alert,
        CancellationToken ct
    )
    {
        var count = alert switch
        {
            AlertType.ParentsLonely => await _repo.ParentsLonelyCountAsync(ct),
            AlertType.PendingCharges => await _repo.PendingChargesCountAsync(ct),
            AlertType.PendingDocuments => await _repo.PendingDocumentsCountAsync(ct),
            AlertType.WithoutGodparents => await _repo.WithoutGodparentsCountAsync(ct),
            _ => -1
        };
        await _cache.SetAsync($"alert:{alert}", count, ct, TimeSpan.FromMinutes(60));
        return count;
    }

    public async Task<Result<AlertDTO>> HandleAsync(
        GetAlertCountQuery query,
        CancellationToken cancellationToken
    )
    {
        var current = await _cache.GetAsync<int>($"alert:{query.Alert}", cancellationToken);
        var count = current is null
            ? await QueryAsync(query.Alert, cancellationToken)
            : current.Value;
        var message = query.Alert switch
        {
            AlertType.ParentsLonely => "Padres/padrinos sin hijos/ahijados",
            AlertType.PendingCharges => "Confirmandos con cobros pendientes",
            AlertType.PendingDocuments => "Confirmandos con documentos pendientes de entregar",
            AlertType.WithoutGodparents => "Confirmandos sin padrinos",
            _ => null
        };

        if (count == -1 || message is null)
            return Result<AlertDTO>.Failure("La alerta no existe");

        return Result<AlertDTO>.Success(new(
            count,
            message
        ));
    }
}