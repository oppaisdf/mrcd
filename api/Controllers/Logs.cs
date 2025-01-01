using api.Models.Filters;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize(Roles = "adm")]
[ApiController]
[Route("api/[controller]")]
public class LogsController(
    ILogService service
) : ControllerBase
{
    private readonly ILogService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        short page = 1,
        string? userId = null,
        short? action = null,
        DateTime? start = null,
        DateTime? end = null
    )
    {
        if (string.IsNullOrWhiteSpace(userId)) userId = null;
        if (start != null && end != null)
        {
            if (end < start) start = null;
            if (end > DateTime.UtcNow) start = null;
            if (start < DateTime.UtcNow.AddMonths(-1))
                return this.DefaultBadRequest("No puede consultar más de un mes, consulte al administrador por más recursos");
        }
        else
        {
            start = null;
            end = null;
        }
        try
        {
            var filters = new LogFilter
            {
                Page = page,
                UserId = userId,
                Action = action,
                Start = start,
                End = end
            };
            var (logs, pages) = await _service.GetAsync(filters);
            return this.DefaultOk(logs, pages);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener logs: {e.Message}"); }
    }

    [HttpGet("Filters")]
    public async Task<IActionResult> GetFiltersAsync()
    {
        try
        {
            var filters = await _service.GetFiltersAsync();
            return this.DefaultOk(filters);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener filtros de logs: {e.Message}"); }
    }
}