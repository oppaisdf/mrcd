using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.AddBaseEntity;
using MRCD.Application.BaseEntity.DelBaseEntity;
using MRCD.Application.Person.AssignPersonEntity;
using MRCD.Domain.Document;

namespace MRCD.API.Endpoints;

internal static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/document")
            .WithTags("Documents");

        app.MapPost("", async (
            [FromBody] BaseEntityRequest request,
            [FromServices] ICommandHandler<AddBaseEntityCommand, Guid, Document> handler,
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
                id => Results.Created($"/api/v2/document/{id}", id),
                conflictWhen: e => e.Contains("en uso")
            );
        })
        .WithName("AddDocument")
        .WithDisplayName("POST /DOcument")
        .WithSummary("Agregar documento")
        .WithDescription("Crea un documento")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization("perm:Document.Write");

        app.MapDelete("{documentId}", async (
            Guid documentId,
            [FromServices] IBaseCommandHandler<DelBaseEntityCommand, Document> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelBaseEntityCommand(
                userId,
                documentId
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelDocument")
        .WithDisplayName("DELETE /DOcument")
        .WithSummary("Eliminar documento")
        .WithDescription("Elimina un documento")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Document.Delete");

        app.MapGet("", async (
            [FromServices] IBaseQueryHandler<Document> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                d => Results.Ok(d)
            );
        })
        .WithName("GetDocument")
        .WithDisplayName("GET /DOcument")
        .WithSummary("Obtener documentos")
        .WithDescription("Retorna listado de documentos")
        .WithOpenApi()
        .Produces<IEnumerable<Document>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Document.Read");

        app.MapPost("{documentId}/person/{personId}", async (
            Guid documentId,
            Guid personId,
            [FromServices] IBaseCommandHandler<AssignPersonEntityCommand, Document> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignPersonEntityCommand(
                personId,
                documentId,
                true,
                 Application.BaseEntity.Common.BaseEntityType.Document
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("ya ha sido")
            );
        })
        .WithName("AssignPersonDocument")
        .WithDisplayName("POST /PersonDocument")
        .WithSummary("Asigna documento a persona")
        .WithDescription("Crea relación entre un confirmando y un documento")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequireAuthorization("perm:Person.Write");

        app.MapDelete("{documentId}/person/{personId}", async (
            Guid documentId,
            Guid personId,
            [FromServices] IBaseCommandHandler<AssignPersonEntityCommand, Document> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignPersonEntityCommand(
                personId,
                documentId,
                false,
                 Application.BaseEntity.Common.BaseEntityType.Document
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("se ha registrado")
            );
        })
        .WithName("UnassignPersonDocument")
        .WithDisplayName("DELETE /PersonDocument")
        .WithSummary("Desasigna documento a persona")
        .WithDescription("Elimina la relación entre un confirmando y un documento")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Person.Write");
    }
}