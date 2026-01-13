using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.User.AddUser;
using MRCD.Application.User.AssignRole;
using MRCD.Application.User.DTOs;
using MRCD.Application.User.GetUserById;
using MRCD.Application.User.UpdateUser;

namespace MRCD.API.Endpoints;

internal static class UserEndpoints
{
    public static void MapUserEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/user")
            .WithTags("User");

        app.MapPost("", async (
            [FromBody] AddUserRequest request,
            ICommandHandler<AddUserCommand, Guid> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                return ResultsMapper.Unauthorized();

            var command = new AddUserCommand(
                userId,
                request.Username,
                request.Password,
                request.Roles
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Created($"api/v2/user/{r}", r),
                e => e.Contains("encontraron"),
                e => e.Contains("ya está")
            );
        })
        .WithName("AddUser")
        .WithDisplayName("POST /AddUser")
        .WithSummary("Agregar usuario")
        .WithDescription("Crea un usuario al sistema")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/assignrole", async (
            [FromQuery] Guid userId,
            [FromQuery] Guid roleId,
            ICommandHandler<AssignRoleCommand> handler,
            CancellationToken ct
        ) =>
        {
            var command = new AssignRoleCommand(
                userId,
                roleId,
                true
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Created(),
                e => e.Contains("no existe"),
                e => e.Contains("ya ha sido")
            );
        })
        .WithName("AssignRoleToUser")
        .WithDisplayName("POST /AssignRoleToUser")
        .WithSummary("Asignar rol a usuario")
        .WithDescription("Asigna un rol a un usuario activo")
        .WithOpenApi()
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        app.MapGet("", async (
            [FromServices] IQueryHandler<IEnumerable<UserDTO>> handler,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r)
            );
        })
        .WithName("GetUsers")
        .WithDisplayName("GET /GetUsers")
        .WithSummary("Obtiene usuarios")
        .WithDescription("Obtiene todos los usuarios con sus roles")
        .WithOpenApi()
        .Produces<IEnumerable<UserDTO>>(StatusCodes.Status201Created);

        app.MapGet("{userId}", async (
            Guid userId,
            IQueryHandler<UserDTO, GetUserByIdQuery> handler,
            CancellationToken ct
        ) =>
        {
            var query = new GetUserByIdQuery(userId);
            var result = await handler.HandleAsync(query, ct);
            return ResultsMapper.ToHttp(
                result,
                r => Results.Ok(r),
                e => e.Contains("existe")
            );
        })
        .WithName("GetUserById")
        .WithDisplayName("GET /GetUserById")
        .WithSummary("Obtiene usuario por ID")
        .WithDescription("Obtiene al usuario con sus roles por ID")
        .WithOpenApi()
        .Produces<UserDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("{id}", async (
            Guid id,
            [FromBody] UpdateUserRequest request,
            ICommandHandler<UpdateUserCommand> handler,
            ClaimsPrincipal user,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                return ResultsMapper.Unauthorized();
            var command = new UpdateUserCommand(
                userId,
                id,
                request.Username,
                request.Password,
                request.IsActive
            );
            var result = await handler.HandleAsync(command, ct);
            return ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("existe"),
                e => e.Contains("en uso")
            );
        })
        .WithName("PatchUser")
        .WithDisplayName("PATCH /PatchUser")
        .WithSummary("Actualizar usuario")
        .WithDescription("Actualiza un usuario por ID")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}