using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Alert.DTOs;
using MRCD.Application.Alert.GetAlertCount;

namespace MRCD.API.Endpoints;

internal static class AlertEndpoints
{
    public static void MapAlertEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/alert")
            .WithTags("Alerts")
            .RequireAuthorization("perm:Alert.Read");

        app.MapGet("{alert}", async (
            AlertType alert,
            [FromServices] IQueryHandler<AlertDTO, GetAlertCountQuery> handler,
            CancellationToken ct
        ) =>
        {
            var query = new GetAlertCountQuery(
                alert
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                alert => Results.Ok(alert),
                e => e.Contains("no existe")
            );
        })
        .WithName("GetAlert")
        .WithDisplayName("GET /Alert")
        .WithSummary("Consultar conteo de alerta")
        .WithDescription("Retorna conteo y mensaje de alerta")
        .WithOpenApi()
        .Produces<AlertDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}