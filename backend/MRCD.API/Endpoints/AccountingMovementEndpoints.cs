using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.AddAccountingMovement;
using MRCD.Application.AccountingMovement.DelAccountingMovement;
using MRCD.Application.AccountingMovement.GetAccountingMovement;
using MRCD.Domain.AccountingMovement;

namespace MRCD.API.Endpoints;

internal static class AccountingMovementEndpoints
{
    public static void MapAccountingMovementEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/accountingmovement")
            .WithTags("AccountingMovements");

        app.MapPost("", async (
            [FromBody] AddChargeRequest request,
            ICommandHandler<AddAccountingMovementCommand, Guid> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new AddAccountingMovementCommand(
                userId,
                request.Name,
                request.Amount
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                id => Results.Created($"/api/v2/accountingmovement/{id}", id)
            );
        })
        .WithName("AddAccountingMovement")
        .WithDisplayName("POST /AccountingMovement")
        .WithSummary("Agregar movimiento contable")
        .WithDescription("Agrega un movimiento contable")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequireAuthorization("perm:AccountingMovement.Write");

        app.MapDelete("{id}", async (
            Guid id,
            [FromServices] ICommandHandler<DelAccountingMovementCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var command = new DelAccountingMovementCommand(
                userId,
                id
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelAccountingMovement")
        .WithDisplayName("DELETE /AccountingMovement")
        .WithSummary("Eliminar movimiento contable")
        .WithDescription("Elimina un movimiento contable")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("perm:AccountingMovement.Delete");

        app.MapGet("", async (
            [FromQuery] DateOnly date,
            [FromQuery] bool filterOnlyByYear,
            IQueryHandler<IEnumerable<AccountingMovement>, GetAccountingMovementQuery> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();
            var query = new GetAccountingMovementQuery(
                userId,
                date,
                filterOnlyByYear
            );
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                l => Results.Ok(l)
            );
        })
        .WithName("GetAccountingMovement")
        .WithDisplayName("GET /AccountingMovement")
        .WithSummary("Obtener movimiento contable")
        .WithDescription("Retorna un listado con los movimientos contables")
        .WithOpenApi()
        .Produces<IEnumerable<AccountingMovement>>(StatusCodes.Status200OK)
        .RequireAuthorization("perm:AccountingMovement.Read");
    }
}