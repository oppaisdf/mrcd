using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.AddPermission;
using MRCD.Application.Permission.AssignToRole;
using MRCD.Application.Permission.DelPermission;
using MRCD.Application.Permission.UnassignToRole;

namespace MRCD.API.Endpoints;

internal static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/permission")
            .WithTags("Permission");

        app.MapPost("", async (
            [FromBody] BaseEntityRequest request,
            ICommandHandler<AddPermissionCommand, Guid> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AddPermissionCommand(
                request.Name
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                id => Results.Ok(id),
                conflictWhen: e => e.Contains("ya existe")
            );
        })
        .WithName("AddPermission")
        .WithDisplayName("POST /AddPermission")
        .WithSummary("Agregar permiso")
        .WithDescription("Crea un permiso de entidades")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapPost("{permissionId}/role/{roleId}", async (
            Guid permissionId,
            Guid roleId,
            [FromServices] ICommandHandler<AssignToRoleCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var command = new AssignToRoleCommand(
                userId,
                permissionId,
                roleId
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("ya ha sido")
            );
        })
        .WithName("AssignPermisionRole")
        .WithDisplayName("POST /AssignPermissionRole")
        .WithSummary("Asignar permiso a rol")
        .WithDescription("Asigna un permiso a un rol")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        app.MapDelete("{permissionId}", async (
            Guid permissionId,
            [FromServices] ICommandHandler<DelPermissionCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var command = new DelPermissionCommand(
                userId,
                permissionId
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("AssignPermisionRole")
        .WithDisplayName("POST /AssignPermissionRole")
        .WithSummary("Asignar permiso a rol")
        .WithDescription("Asigna un permiso a un rol")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("", async (
            [FromServices] IQueryHandler<List<Domain.Role.Permission>> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            ResultsMapper.ToHttp(
                result,
                ps => Results.Ok(ps)
            );
        })
        .WithName("GetPermissions")
        .WithDisplayName("GET /Permissions")
        .WithSummary("Obtener permisos")
        .WithDescription("Retorna listado de permisos")
        .WithOpenApi()
        .Produces<IEnumerable<Domain.Role.Permission>>(StatusCodes.Status200OK);

        app.MapPost("{permissionId}/role/{roleId}", async (
            Guid permissionId,
            Guid roleId,
            [FromServices] ICommandHandler<UnassignToRoleCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var command = new UnassignToRoleCommand(
                userId,
                roleId,
                permissionId
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe")
            );
        })
        .WithName("DelPermissionRole")
        .WithDisplayName("DELETE /PermissionRole")
        .WithSummary("Eliminar permission role")
        .WithDescription("Desasigna un permiso a un rol")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}