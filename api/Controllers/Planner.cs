using System.Security.Claims;
using api.Common;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlannerController(
    IPlannerService service
) : ControllerBase
{
    private readonly IPlannerService _service = service;

    [HttpGet("{year}/{month}")]
    public async Task<IActionResult> GetAsync(
        ushort year,
        ushort month
    )
    {
        if (year > DateTime.UtcNow.Year)
            return this.DefaultBadRequest("No llegamos a ese año :c");
        if (year < 2020)
            return this.DefaultBadRequest("No hay registros de ese año :c");
        if (month > 12 || month < 1)
            return this.DefaultBadRequest("El mes no existe :c");
        try
        {
            var activities = await _service.GetAsync(year, month);
            return this.DefaultOk(activities);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar actividades del mes {month} en año {year}: {e.Message}"); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        uint id
    )
    {
        try
        {
            var activity = await _service.GetByIdAsync(id);
            return this.DefaultOk(activity);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al retornar actividad {id}: {e.Message}"); }
    }

    [HttpPost("Activity")]
    public async Task<IActionResult> CreateActivityAsync(
        [FromBody] ActivityRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("El nombre de la actividad es requerido");
        else request.Name = request.Name.Trim();
        if (request.Date.Year != DateTime.UtcNow.Year)
            return this.DefaultBadRequest("No se pueden agregar actividades fuera del año en curso");
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var id = await _service.CreateActivityAsync(userId, request);
            return this.DefaultOk(new { Id = id }, "Se ha creado la actividad satisfactoriamente c:");
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear actividad: {e.Message}"); }
    }

    [HttpPost("Stage")]
    public async Task<IActionResult> CreateStageAsync(
        [FromBody] StageRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("El nombre de la etapa es requerida");
        else request.Name = request.Name.Trim();

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.CreateStageAsync(userId, request);
            return this.DefaultOk(new { }, "Se ha creado la etapa satisfactoriamente :3");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear etapa {request.Name}: {e.Message}"); }
    }

    [HttpPost("ActivityStage")]
    public async Task<IActionResult> AddStageToActivity(
        [FromBody] ActivityStageRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Notes)) request.Notes = null;
        else request.Notes = request.Notes.Trim();
        if (string.IsNullOrWhiteSpace(request.UserId)) request.UserId = null;
        else request.UserId = request.UserId.Trim();

        try
        {
            await _service.AddStageToActivityAsync(request);
            return this.DefaultOk(new { }, "Se ha asignado la etapa correctamente a la actividad :3");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al asignar stage {request.StageId} a actividad {request.ActivityId}: {e.Message}"); }
    }
}