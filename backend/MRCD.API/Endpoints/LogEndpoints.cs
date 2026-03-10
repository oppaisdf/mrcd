using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Logs.DTOs;
using MRCD.Application.Logs.GetLogs;

namespace MRCD.API.Endpoints;

internal static class LogEndpoints
{
    public static void MapLogEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/logs")
            .WithTags("Logs");

        app.MapGet("", async (
            [FromQuery] uint page,
            [FromServices] IQueryHandler<Pagination<LogDTO>, GetLogsQuery> handler,
            CancellationToken ct
        ) =>
        {
            var query = new GetLogsQuery(
                30,
                page
            );
            var paginated = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                paginated,
                p => Results.Ok(p)
            );
        })
        .WithName("GetLogs")
        .WithDisplayName("GET /Logs")
        .WithSummary("Obtener logs")
        .WithDescription("Lista de logs paginados")
        .WithOpenApi()
        .Produces<Pagination<LogDTO>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Log.Read"); ;
    }
}