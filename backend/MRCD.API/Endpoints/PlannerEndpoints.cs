using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.AddActivity;
using MRCD.Application.Planner.AssignStageToActivity;
using MRCD.Application.Planner.DelActivity;
using MRCD.Application.Planner.DTOs;
using MRCD.Application.Planner.GetActivity;
using MRCD.Application.Planner.GetCalendar;

namespace MRCD.API.Endpoints;

internal static class PlannerEndpoints
{
    public static void MapPlannerEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/planner")
            .WithTags("Planner");

        #region Activity
        app.MapPost("/activity", async (
            [FromBody] AddActivityRequest request,
            ICommandHandler<AddActivityCommand, Guid> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var command = new AddActivityCommand(
                userId,
                request.Name,
                request.Date
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                id => Results.Created($"/api/v2/planner/activity/{id}", id)
            );
        })
        .WithName("AddActivity")
        .WithDisplayName("POST /AddActivity")
        .WithSummary("Agregar actividad")
        .WithDescription("Crea una actividad en la agenda")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status201Created);

        app.MapDelete("/activity/{id}", async (
            Guid id,
            [FromServices] ICommandHandler<DelActivityCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();

            var command = new DelActivityCommand(
                userId,
                id
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelActivity")
        .WithDisplayName("DELETE /DelActivity")
        .WithSummary("Eliminar actividad")
        .WithDescription("Elimina una actividad de la agenda")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/activity/{id}", async (
            Guid id,
            [FromServices] IQueryHandler<ActivityDTO, GetActivityQuery> handler,
            CancellationToken ct
        ) =>
        {
            var query = new GetActivityQuery(id);
            var result = await handler.HandleAsync(query, ct);
            ResultsMapper.ToHttp(
                result,
                a => Results.Ok(a),
                e => e.Contains("no existe")
            );
        })
        .WithName("GetActivityById")
        .WithDisplayName("GET /GetActivityById")
        .WithSummary("Obtiene actividad by Id")
        .WithDescription("Obtiene una actividad con detalles por Id")
        .WithOpenApi()
        .Produces<ActivityDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
        #endregion

        app.MapGet("/calendar", async (
            [FromQuery] ushort year,
            [FromQuery] ushort month,
            [FromServices] IQueryHandler<IEnumerable<CalendarDTO>, GetCalendarQuery> handler,
            CancellationToken ct
        ) =>
        {
            var query = new GetCalendarQuery(
                year,
                month
            );
            var result = await handler.HandleAsync(query, ct);
            ResultsMapper.ToHttp(
                result,
                c => Results.Ok(c)
            );
        })
        .WithName("GetCalendar")
        .WithDisplayName("GET /GetCalendar")
        .WithSummary("Obtiene calendario de actividades")
        .WithDescription("Obtiene actividades de todo el mes")
        .WithOpenApi()
        .Produces<IEnumerable<CalendarDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapPost("/stage/activity", async (
            [FromBody] AssignStageToActivityRequest request,
            ICommandHandler<AssignStageToActivityCommand> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignStageToActivityCommand(
                request.StageId,
                request.ActivityId,
                true,
                request.IsUserMain,
                request.UserId,
                request.Notes
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Created(),
                e => e.Contains("no existe"),
                e => e.Contains("ya ha sido")
            );
        })
        .WithName("AssignStageToActivity")
        .WithDisplayName("POST /AssignStageToActivity")
        .WithSummary("Asignar fase de actividad")
        .WithDescription("Asigna una fase de actividad a una actividad")
        .WithOpenApi()
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapDelete("/stage/{stageId}/activity/{activityId}/user/{userId}", async (
            Guid stageId,
            Guid activityId,
            Guid? userId,
            [FromServices] ICommandHandler<AssignStageToActivityCommand> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignStageToActivityCommand(
                stageId,
                activityId,
                false,
                false,
                userId,
                null
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no ha sido")
            );
        })
        .WithName("DelAssignStageToActivity")
        .WithDisplayName("DELETE /DelAssignStageToActivity")
        .WithSummary("Elimminar fase de actividad")
        .WithDescription("Desasgna una fase de actividad a una actividad")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}