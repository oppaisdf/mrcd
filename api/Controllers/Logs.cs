using api.Common;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize(Roles = "sys")]
[ApiController]
[Route("api/[controller]")]
public class LogsController(
    ILogService service
) : ControllerBase
{
    private readonly ILogService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var actions = await _service.GetAsync();
            return this.DefaultOk(actions);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener aciones de logs: {e.Message}"); }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ActionLogRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return this.DefaultBadRequest("El nombre es requerido");
        request.Name = request.Name.Trim();
        try
        {
            await _service.CreateAsync(request.Name);
            return this.DefaultOk(new { }, "Ación creada satisfactoriamente");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error to create action log: {e.Message}"); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        short id,
        [FromBody] ActionLogRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return this.DefaultBadRequest("El nombre es requerido");
        request.Name = request.Name.Trim();
        try
        {
            await _service.UpdateAsync(id, request.Name);
            return this.DefaultOk(new { }, "Ación actualizada satisfactoriamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error to update action log {id}: {e.Message}"); }
    }
}