namespace MRCD.API.Endpoints;

internal static class PublicEndpoints
{
    public static void MapPublicEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/")
            .WithTags("public");

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
    }
}