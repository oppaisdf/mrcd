using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Alert.DTOs;
using MRCD.Application.Alert.Get.AlertCount;
using MRCD.Application.Common;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Parent.GetParent;
using MRCD.Application.Person.DTOs;
using MRCD.Application.Person.GetPerson;

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

        app.MapGet("/people/charges", async (
            [FromQuery] ushort page,
            [FromServices] IQueryHandler<Pagination<SimplePersonDTO>, GetPersonQuery> handler,
            ClaimsPrincipal user,
            CancellationToken ct,
            string? name = null,
            bool? isSunday = null,
            bool? isMasculine = null
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var query = new GetPersonQuery(
                userId,
                IsActive: true,
                page,
                name,
                isSunday,
                isMasculine,
                Alert: AlertType.PendingCharges
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r)
            );
        })
        .WithName("GetPeoplePendingCharges")
        .WithDisplayName("GET /PeoplePendingCharges")
        .WithSummary("Obtiene listado simple de cobros pendientes")
        .WithDescription("Retorna listado simple de confirmandos con pagos pendientes")
        .WithOpenApi()
        .Produces<Pagination<SimplePersonDTO>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Person.Read");

        app.MapGet("/people/documents", async (
            [FromQuery] ushort page,
            [FromServices] IQueryHandler<Pagination<SimplePersonDTO>, GetPersonQuery> handler,
            ClaimsPrincipal user,
            CancellationToken ct,
            string? name = null,
            bool? isSunday = null,
            bool? isMasculine = null
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var query = new GetPersonQuery(
                userId,
                IsActive: true,
                page,
                name,
                isSunday,
                isMasculine,
                Alert: AlertType.PendingDocuments
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r)
            );
        })
        .WithName("GetPeoplePendingDocuments")
        .WithDisplayName("GET /PeoplePendingDocuments")
        .WithSummary("Obtiene listado simple de documentos pendientes")
        .WithDescription("Retorna listado simple de confirmandos con documentes pendientes")
        .WithOpenApi()
        .Produces<Pagination<SimplePersonDTO>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Person.Read");

        app.MapGet("/people/godparents", async (
            [FromQuery] ushort page,
            [FromServices] IQueryHandler<Pagination<SimplePersonDTO>, GetPersonQuery> handler,
            ClaimsPrincipal user,
            CancellationToken ct,
            string? name = null,
            bool? isSunday = null,
            bool? isMasculine = null
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var query = new GetPersonQuery(
                userId,
                IsActive: true,
                page,
                name,
                isSunday,
                isMasculine,
                Alert: AlertType.WithoutGodparents
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r)
            );
        })
        .WithName("GetPeoplePendingGodparents")
        .WithDisplayName("GET /PeoplePendingGodparents")
        .WithSummary("Obtiene listado simple de padrinos pendientes")
        .WithDescription("Retorna listado simple de confirmandos sin padrinos")
        .WithOpenApi()
        .Produces<Pagination<SimplePersonDTO>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Person.Read");
    }
}