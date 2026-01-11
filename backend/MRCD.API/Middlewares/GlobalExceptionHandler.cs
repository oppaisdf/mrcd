using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MRCD.API.Middlewares;

internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetails
) : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails = problemDetails;

    public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception ex, CancellationToken ct)
    {
        // Clasificación mínima de excepciones conocidas de infra
        var (status, title) = ex switch
        {
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Conflicto de concurrencia"),
            DbUpdateException => (StatusCodes.Status409Conflict, "Conflicto al persistir datos"),
            OperationCanceledException => (StatusCodes.Status499ClientClosedRequest, "Solicitud cancelada"), // 499 no estándar, pero útil
            TimeoutException => (StatusCodes.Status504GatewayTimeout, "Timeout de operación"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor :c")
        };

        // Log estructurado: aquí se deja que el stack de logging (Serilog) capture ex con detalles.
        // No retornar el stack al cliente.
        http.Response.StatusCode = status;
        http.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Instance = http.TraceIdentifier
        };

        problem.Extensions["errorId"] = http.TraceIdentifier;

        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = http,
            ProblemDetails = problem,
            Exception = ex
        });
    }
}
