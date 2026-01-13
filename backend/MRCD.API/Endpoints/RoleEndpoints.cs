using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.AddRole;
using MRCD.Application.Role.DTOs;

namespace MRCD.API.Endpoints;

internal static class RoleEndpoints
{
    public static void MapRoleEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/role")
            .WithTags("Roles");

        app.MapPost("", async (
            [FromBody] AddRoleRequest request,
            ICommandHandler<AddRoleCommand, Guid> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
                return ResultsMapper.Unauthorized();

            var command = new AddRoleCommand(
                userId,
                request.Name
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Created($"/api/v2/role/{r}", r),
                conflictWhen: e => e.Contains("en uso")
            );
        })
        .WithName("AddRole")
        .WithDisplayName("POST /AddRole")
        .WithSummary("Agregar rol de usuario")
        .WithDescription("Crea un rol de usuario")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapGet("", async (
            [FromServices] IQueryHandler<List<Domain.Role.Role>> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r)
            );
        })
        .WithName("GetRoles")
        .WithDisplayName("GET /GetRole")
        .WithSummary("Obtener roles")
        .WithDescription("Retorna todos los raw roles")
        .WithOpenApi()
        .Produces<IEnumerable<Domain.Role.Role>>(StatusCodes.Status200OK);

        app.MapGet("/permission", async (
            [FromServices] IQueryHandler<IEnumerable<RoleWithPermissionDTO>> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r)
            );
        })
        .WithName("GetRolePermission")
        .WithDisplayName("GET /RolePermission")
        .WithSummary("Obtener roles con permisos")
        .WithDescription("Obtiene los roles con sus permisos asignados")
        .WithOpenApi()
        .Produces<IEnumerable<RoleWithPermissionDTO>>(StatusCodes.Status200OK);
    }
}