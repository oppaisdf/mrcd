using MRCD.API.Endpoints;
using MRCD.API.Extensions;
using MRCD.API.Services;
using MRCD.Application;
using MRCD.Application.Abstracts.Security;
using MRCD.Infrastructure;
using Serilog;
using Serilog.Events;

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
var jwt = new TokenOptions(
    Environment.GetEnvironmentVariable("JWT_ISSUER")
        ?? builder.Configuration["JWT_ISSUER"]
        ?? throw new InvalidOperationException("=== No se encontró ISSUER de token ==="),
    Environment.GetEnvironmentVariable("JWT_AUDIENCE")
        ?? builder.Configuration["JWT_AUDIENCE"]
        ?? throw new InvalidOperationException("=== No se encontró AUDIENCE de token ==="),
    Environment.GetEnvironmentVariable("JWT_KEY")
        ?? builder.Configuration["JWT_KEY"]
        ?? throw new InvalidOperationException("=== No se encontró KEY de token ==="),
    30,
    5
);

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

builder.ConfigureJWT(jwt);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Pomelo.EntityFrameworkCore.MySql", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.MySQL(connectionString: connection, tableName: "logs")
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    await next();
});

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
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
app.MapDegreeEndpoints();
app.MapParentEndpoints();
app.MapAlertEndpoints();
app.MapLogEndpoints();
app.Run();
