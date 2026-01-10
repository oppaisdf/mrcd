var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/health", () =>
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
.WithOpenApi()
.WithTags("Public");

app.Run();
