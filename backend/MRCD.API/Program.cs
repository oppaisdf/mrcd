using MRCD.API.Endpoints;
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
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Instance ??= ctx.HttpContext.TraceIdentifier;
        if (ctx.ProblemDetails.Status is StatusCodes.Status500InternalServerError)
        {
            ctx.ProblemDetails.Title ??= "Internal server error :c";
            ctx.ProblemDetails.Detail ??= "Error inesperado del servidor. Intenta de nuevo o consulta a soporte.";
        }
    };
});

if (builder.Environment.IsDevelopment())
    builder.Services.AddDistributedMemoryCache();
else builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Environment.GetEnvironmentVariable("REDIS")
        ?? builder.Configuration.GetConnectionString("REDIS")
        ?? throw new InvalidOperationException("=== No se encontró la conexión a Redis ===");
    options.InstanceName = "mrcd:";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.MapPublicEndpoints();
app.MapUserEndpoints();
app.MapRoleEndpoints();
app.MapPlannerEndpoints();
app.MapPersonEndpoints();
app.MapPermissionEndpoints();
app.MapChargeEndpoints();
app.MapAttendanceEndpoints();
app.MapAccountingMovementEndpoints();
app.MapDocumentEndpoints();
app.MapSacramentEndpoints();
app.Run();
