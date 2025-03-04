using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AlertController(
    IAlertService service
) : ControllerBase
{
    private readonly IAlertService _service = service;

    [HttpGet("{alert}")]
    public async Task<IActionResult> GetCountAsync(
        ushort alert
    )
    {
        if (alert > 3) return this.DefaultBadRequest("La alerta no existe");
        try
        {
            var count = await _service.CountAsync(alert);
            return this.DefaultOk(new { }, $"{count}");
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener cuenta de alerta {alert}: {e.Message}"); }
    }

    [HttpGet("{alert}/Full")]
    public async Task<IActionResult> GetAsync(
        ushort alert
    )
    {
        if (alert > 3) return this.DefaultBadRequest("La alerta no existe");
        try
        {
            var people = await _service.GetAsync(alert);
            return this.DefaultOk(people);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener listado de alerta {alert}: {e.Message}"); }
    }
}