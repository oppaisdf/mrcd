using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.DelBaseEntity;
using MRCD.Application.Charge.AddCharge;
using MRCD.Domain.Charge;

namespace MRCD.API.Endpoints;

internal static class ChargeEndpoints
{
    public static void MapChargeEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/charge")
            .WithTags("Charges");

        app.MapPost("", async (
            [FromBody] AddChargeRequest request,
            ICommandHandler<AddChargeCommand, Guid> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new AddChargeCommand(
                userId,
                request.Name,
                request.Amount
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                id => Results.Created($"/api/v2/charge/{id}", id),
                conflictWhen: e => e.Contains("en uso")
            );
        })
        .WithName("AddCharge")
        .WithDisplayName("POST /Charge")
        .WithSummary("Agregar cobro")
        .WithDescription("Crea un cobro")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization("perm:Charge.Write");

        app.MapDelete("{chargeId}", async (
            Guid chargeId,
            [FromServices] IBaseCommandHandler<DelBaseEntityCommand, Charge> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelBaseEntityCommand(
                userId,
                chargeId
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelCharge")
        .WithDisplayName("DELETE /Charge")
        .WithSummary("Eliminar cobro")
        .WithDescription("Elimina un cobro")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:Charge.Delete");

        app.MapGet("", async (
            [FromServices] IBaseQueryHandler<Charge> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                cs => Results.Ok(cs)
            );
        })
        .WithName("GetCharge")
        .WithDisplayName("GET /Charge")
        .WithSummary("Obtener cobros")
        .WithDescription("Retornba listado de cobros")
        .WithOpenApi()
        .Produces<IEnumerable<Charge>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:Charge.Read");
    }
}