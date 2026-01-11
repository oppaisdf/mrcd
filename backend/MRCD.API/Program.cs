using MRCD.Application;
using MRCD.Application.Abstracts.Security;
using MRCD.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var connection = Environment.GetEnvironmentVariable("DB_CONNECTION")
    ?? builder.Configuration.GetConnectionString("DB_CONNECTION")
    ?? throw new InvalidOperationException("==== No se encontró la cadena de conexión :c ====");
var encryptionOptions = new EncryptionOptions()
{
    KeyBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")
        ?? builder.Configuration["ENCRYPTION_KEY"]
        ?? throw new InvalidOperationException("==== No se encontró la llave de encriptación :c ===="),
    Version = "v1"
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrasctructure(connection, encryptionOptions);

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
