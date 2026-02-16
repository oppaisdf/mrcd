using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Parent.AddParent;
using MRCD.Application.Parent.AssignParent;
using MRCD.Application.Parent.DelParent;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Parent.GetParent;

namespace MRCD.API.Endpoints;

internal static class ParentEndpoints
{
    public static void MapParentEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/parent")
            .WithTags("Parents");

        app.MapPost("", async (
            [FromBody] CreateParentRequest request,
            ICommandHandler<AddParentCommand, Guid> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new AddParentCommand(
                userId,
                request.ParentName,
                request.IsMasculine,
                request.IsParent,
                request.Phone,
                request.PersonId
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                id => Results.Created($"/api/v2/document/{id}", id),
                e => e.Contains("no existe"),
                e => e.Contains("ya se ha")
            );
        })
        .WithName("AddParent")
        .WithDisplayName("POST /Parent")
        .WithSummary("Agregar padre/padrino")
        .WithDescription("Crea un padre o padrino")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization("perm:Parent.Write");

        app.MapGet("", async (
            [FromQuery] ushort page,
            [FromServices] IQueryHandler<Pagination<ParentDTO>, GetParentQuery> handler,
            CancellationToken ct,
            string? parentName = null
        ) =>
        {
            var query = new GetParentQuery(
                page,
                20,
                parentName
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                parents => Results.Ok(parents)
            );
        })
        .WithName("GetParent")
        .WithDisplayName("GET /Parent")
        .WithSummary("Obtener padres/padrinos")
        .WithDescription("Retorna listado de padres/padrinos paginado")
        .WithOpenApi()
        .Produces<Pagination<ParentDTO>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Parent.Read");

        app.MapDelete("{parentId}", async (
            Guid parentId,
            [FromServices] ICommandHandler<DelParentCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelParentCommand(
                userId,
                parentId
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DeleteParent")
        .WithDisplayName("DELETE /Parent")
        .WithSummary("Elimina padre/padrino")
        .WithDescription("Elimina un padre/padrino")
        .WithOpenApi()
        .Produces<Pagination<ParentDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Parent.Delete");

        app.MapPost("/person", async (
            [FromBody] AssignParentRequest request,
            [FromServices] ICommandHandler<AssignParentCommand> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignParentCommand(
                request.ParentId,
                request.PersonId,
                request.IsParent,
                true
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("se ha")
            );
        })
        .WithName("AssignParent")
        .WithDisplayName("POST /ParentPerson")
        .WithSummary("Asociar padre/padrino")
        .WithDescription("Asocia un padre/padrino a un confirmando")
        .WithOpenApi()
        .Produces<Pagination<ParentDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequireAuthorization("perm:Parent.Write");

        app.MapDelete("/person/{personId}/parent/{parentId}/type/{isParent}", async (
            Guid personId,
            Guid parentId,
            bool isParent,
            [FromServices] ICommandHandler<AssignParentCommand> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignParentCommand(
                parentId,
                personId,
                isParent,
                false
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("se ha")
            );
        })
        .WithName("UnassignParent")
        .WithDisplayName("DELETE /ParentPerson")
        .WithSummary("Desasociar padre/padrino")
        .WithDescription("Desasocia un padre/padrino a un confirmando")
        .WithOpenApi()
        .Produces<Pagination<ParentDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Parent.Delete");
    }
}