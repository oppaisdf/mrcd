using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Alert.DTOs;
using MRCD.Application.Alert.Get.AlertCount;
using MRCD.Application.Common;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Parent.GetParent;

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

        app.MapGet("/parents", async (
            [FromQuery] ushort page,
            [FromServices] IQueryHandler<Pagination<ParentDTO>, GetParentQuery> handler,
            CancellationToken ct,
            string? parentName = null
        ) =>
        {
            var query = new GetParentQuery(
                page,
                20,
                parentName,
                AlertType.ParentsLonely
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                parents => Results.Ok(parents)
            );
        })
        .WithName("GetParentAlert")
        .WithDisplayName("GET /ParentAlert")
        .WithSummary("Obtener padres/padrinos sin hijos/ahijados")
        .WithDescription("Retorna listado de padres/padrinos paginado sin hijos/ahijados")
        .WithOpenApi()
        .Produces<Pagination<ParentDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization("perm:Parent.Read");
    }
}