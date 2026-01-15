using Microsoft.AspNetCore.Mvc;
using MRCD.API.DTOs;
using MRCD.API.Services;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.User.DTOs;
using MRCD.Application.User.Login;

namespace MRCD.API.Endpoints;

internal static class PublicEndpoints
{
    public static void MapPublicEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/")
            .WithTags("Public");

        app.MapGet("/health", () =>
        {
            return Results.Ok(new
            {
                Status = "All right! :3"
            });
        })
        .WithName("GetHealth")
        .WithDisplayName("Health API")
        .WithSummary("Verifica salud API")
        .WithDescription("Verifica si la API responde correctamente")
        .Produces(StatusCodes.Status200OK)
        .WithOpenApi();

        app.MapPost("/auth/login", async (
            [FromBody] UserLoginCommand request,
            [FromServices] ICommandHandler<UserLoginCommand, LoginDTO> handler,
            [FromServices] ITokenService jwt,
            CancellationToken ct
        ) =>
        {
            var result = await handler.HandleAsync(request, ct);
            if (!result.IsSuccess)
                return Results.Problem(
                    title: "Bad Request",
                    detail: result.Error,
                    statusCode: StatusCodes.Status400BadRequest
                );
            var token = jwt.Create(
                result.Value!.UserId,
                result.Value.Roles
            );
            return Results.Ok(new TokenDTO(
                token.AccessToken,
                token.ExpiresAtUTC
            ));
        });
    }
}