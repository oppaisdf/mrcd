using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Alert.Contracts;
using MRCD.Application.Alert.DTOs;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services.Attendance;
using MRCD.Domain.Common;

namespace MRCD.Application.Alert.Get.AlertCount;

internal sealed class GetAlertCountHandler(
    IAlertRepository repo,
    ICacheService cache,
    IPersonRepository person,
    IAttendanceRepository attendance,
    IAttendanceService attService
) : IQueryHandler<AlertDTO, GetAlertCountQuery>
{
    private readonly IAlertRepository _repo = repo;
    private readonly ICacheService _cache = cache;
    private readonly IPersonRepository _person = person;
    private readonly IAttendanceRepository _attendance = attendance;
    private readonly IAttendanceService _attService = attService;

    private async Task<int> GetAttendanceCountAsync(
        CancellationToken ct
    )
    {
        var attendances = await _attendance.ToListAsync(
            DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)),
            filteredOnlyByYear: true,
            ct
        );
        var activePeople = await _person.OnlyActiveToListAsync(ct);
        var consecutiveFoulsPeople = _attService.GetAlertAttendances(
            activePeople,
            attendances,
            consecutiveFoulsWeekCount: 2
        );
        return consecutiveFoulsPeople.Count;
    }

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
            AlertType.ConsecutiveFouls => await GetAttendanceCountAsync(ct),
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
        var current = await _cache.GetAsync<int?>($"alert:{query.Alert}", cancellationToken);
        var count = current is null
            ? await QueryAsync(query.Alert, cancellationToken)
            : current.Value;
        var message = query.Alert switch
        {
            AlertType.ParentsLonely => "Padres/padrinos sin hijos/ahijados",
            AlertType.PendingCharges => "Confirmandos con cobros pendientes",
            AlertType.PendingDocuments => "Confirmandos con documentos pendientes de entregar",
            AlertType.WithoutGodparents => "Confirmandos sin padrinos",
            AlertType.ConsecutiveFouls => "Confirmandos con las últimas 2 faltas seguidas",
            _ => null
        };
        var route = query.Alert switch
        {
            AlertType.ParentsLonely => "/parents",
            AlertType.PendingCharges => "/people",
            AlertType.PendingDocuments => "/people",
            AlertType.WithoutGodparents => "/people",
            AlertType.ConsecutiveFouls => "/print/attendance",
            _ => ""
        };
        var parameter = query.Alert switch
        {
            AlertType.ParentsLonely => new Dictionary<string, string>{["alert"] = "lonely-parents"},
            AlertType.PendingCharges => new Dictionary<string, string>{["alert"] = "pending-charges"},
            AlertType.PendingDocuments => new Dictionary<string, string>{["alert"] = "pending-documents"},
            AlertType.WithoutGodparents => new Dictionary<string, string>{["alert"] = "pending-godparents"},
            AlertType.ConsecutiveFouls => new Dictionary<string, string>{["alert"] = "consecutive-fouls"},
            _ => null
        };

        if (count == -1 || message is null)
            return Result<AlertDTO>.Failure("La alerta no existe");

        return Result<AlertDTO>.Success(new(
            count,
            message,
            route,
            parameter
        ));
    }
}