using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.AddAttendance;
using MRCD.Application.Attendance.DelAttendance;
using MRCD.Application.Attendance.DTOs;
using MRCD.Application.Attendance.GetAttendance;

namespace MRCD.API.Endpoints;

internal static class AttendanceEndpoints
{
    public static void MapAttendanceEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/attendance")
            .WithTags("Attendance");

        app.MapPost("", async (
            [FromBody] AttendanceRequest request,
            ICommandHandler<AddAttendanceCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct,
            DateOnly? date = null
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new AddAttendanceCommand(
                userId,
                request.PersonId,
                request.IsAttendance,
                date ?? DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6))
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("se ha")
            );
        })
        .WithName("AddAttendance")
        .WithDisplayName("POST /Attendance")
        .WithSummary("Pasar asistencia")
        .WithDescription("Registra asistencia de una persona")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequireAuthorization("perm:Attendance.Write");

        app.MapDelete("{personId}/date/{date}", async (
            Guid personId,
            DateOnly date,
            [FromServices] ICommandHandler<DelAttendanceCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelAttendanceCommand(
                userId,
                personId,
                date
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("se ha pasado")
            );
        })
        .WithName("DelAttendance")
        .WithDisplayName("DELETE /Attendance")
        .WithSummary("Remover asistencia")
        .WithDescription("Elimina asistencia de una persona")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Attendance.Delete");

        app.MapGet("{date}", async (
            DateOnly date,
            [FromQuery] bool filteredOnlyByYear,
            [FromServices] IQueryHandler<IEnumerable<AttendanceDTO>, GetAttendanceQuery> handler,
            CancellationToken ct,
            bool? isSunday = null,
            bool? isMasculine = null,
            string? personName = null
        ) =>
        {
            var query = new GetAttendanceQuery(
                date,
                filteredOnlyByYear,
                isSunday,
                isMasculine,
                personName
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                a => Results.Ok(a)
            );
        })
        .WithName("GetAttendance")
        .WithDisplayName("GET /Attendance")
        .WithSummary("Obtener asistencia")
        .WithDescription("Obtiene listado de asistencias")
        .WithOpenApi()
        .Produces<IEnumerable<AttendanceDTO>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Attendance.Read");
    }
}