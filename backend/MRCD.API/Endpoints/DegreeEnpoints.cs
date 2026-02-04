using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.AddBaseEntity;
using MRCD.Application.BaseEntity.DelBaseEntity;
using MRCD.Domain.Degree;

namespace MRCD.API.Endpoints;

internal static class DegreeEndpoints
{
    public static void MapDegreeEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/degree")
            .WithTags("Degrees");

        app.MapPost("", async (
            [FromBody] BaseEntityRequest request,
            [FromServices] ICommandHandler<AddBaseEntityCommand, Guid, Degree> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new AddBaseEntityCommand(
                userId,
                request.Name
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                id => Results.Created($"/api/v2/degree/{id}", id),
                conflictWhen: e => e.Contains("en uso")
            );
        })
        .WithName("AddDegree")
        .WithDisplayName("POST /Degree")
        .WithSummary("Agregar grado")
        .WithDescription("Crea un grado académco")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization("perm:Degree.Write");

        app.MapDelete("{degreeId}", async (
            Guid degreeId,
            [FromServices] IBaseCommandHandler<DelBaseEntityCommand, Degree> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelBaseEntityCommand(
                userId,
                degreeId
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelDegree")
        .WithDisplayName("DELETE /Degree")
        .WithSummary("Eliminar grado")
        .WithDescription("Elimina un grado académico")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Degree.Delete");

        app.MapGet("", async (
            [FromServices] IBaseQueryHandler<Degree> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                d => Results.Ok(d)
            );
        })
        .WithName("GetDegree")
        .WithDisplayName("GET /Degree")
        .WithSummary("Obtener grados")
        .WithDescription("Retorna listado de grados académicos")
        .WithOpenApi()
        .Produces<IEnumerable<Degree>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Degree.Read");
    }
}