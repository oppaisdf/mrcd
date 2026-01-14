using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.AddBaseEntity;
using MRCD.Application.BaseEntity.DelBaseEntity;
using MRCD.Application.Person.AssignPersonEntity;
using MRCD.Domain.Sacrament;

namespace MRCD.API.Endpoints;

internal static class SacarmentEndpoints
{
    public static void MapSacramentEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/sacrament")
            .WithTags("Sacraments");

        app.MapPost("", async (
            [FromBody] BaseEntityRequest request,
            [FromServices] ICommandHandler<AddBaseEntityCommand, Guid, Sacrament> handler,
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
                id => Results.Created($"/api/v2/sacrament/{id}", id),
                conflictWhen: e => e.Contains("en uso")
            );
        })
        .WithName("AddSacrament")
        .WithDisplayName("POST /Sacrament")
        .WithSummary("Agregar sacramento")
        .WithDescription("Crea un sacramento")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapDelete("{sacramentId}", async (
            Guid sacramentId,
            [FromServices] IBaseCommandHandler<DelBaseEntityCommand, Sacrament> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelBaseEntityCommand(
                userId,
                sacramentId
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelSacrament")
        .WithDisplayName("DELETE /Sacrament")
        .WithSummary("Eliminar sacramento")
        .WithDescription("Elimina un sacramento")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("", async (
            [FromServices] IBaseQueryHandler<Sacrament> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                d => Results.Ok(d)
            );
        })
        .WithName("GetSacrament")
        .WithDisplayName("GET /Sacrament")
        .WithSummary("Obtener sacramentos")
        .WithDescription("Retorna listado de sacramentos")
        .WithOpenApi()
        .Produces<IEnumerable<Sacrament>>(StatusCodes.Status200OK);

        app.MapPost("{sacramentId}/person/{personId}", async (
            Guid sacramentId,
            Guid personId,
            [FromServices] IBaseCommandHandler<AssignPersonEntityCommand, Sacrament> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignPersonEntityCommand(
                personId,
                sacramentId,
                true,
                 Application.BaseEntity.Common.BaseEntityType.Sacrament
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("ya ha sido")
            );
        })
        .WithName("AssignPersonSacrament")
        .WithDisplayName("POST /PersonSacrament")
        .WithSummary("Asigna sacramento a persona")
        .WithDescription("Crea relación entre un confirmando y un sacramento")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        app.MapDelete("{sacramentId}/person/{personId}", async (
            Guid sacramentId,
            Guid personId,
            [FromServices] IBaseCommandHandler<AssignPersonEntityCommand, Sacrament> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignPersonEntityCommand(
                personId,
                sacramentId,
                false,
                 Application.BaseEntity.Common.BaseEntityType.Sacrament
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("se ha registrado")
            );
        })
        .WithName("UnassignPersonSacrament")
        .WithDisplayName("DELETE /PersonSacrament")
        .WithSummary("Desasigna sacramento a persona")
        .WithDescription("Elimina la relación entre un confirmando y un sacramento")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}