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

    [HttpGet("{month}")]
    public async Task<IActionResult> GetAsync(
        ushort month
    )
    {
        if (month > 12 || month < 1)
            return this.DefaultBadRequest("El mes no existe :c");
        try
        {
            var activities = await _service.GetAsync((ushort)DateTime.UtcNow.Year, month);
            return this.DefaultOk(activities);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar actividades del mes {month}: {e.Message}"); }
    }

    [HttpGet("Activity/{id}")]
    public async Task<IActionResult> GetByIdAsync(
        uint id
    )
    {
        try
        {
            var activity = await _service.GetByIdAsync(id);
            if (activity == null)
                return this.DefaultNotFound("La actividad que busca no existe");
            return this.DefaultOk(activity);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al retornar actividad {id}: {e.Message}"); }
    }

    [HttpGet("Activity/Next")]
    public async Task<IActionResult> GetNextAcitivyAsync()
    {
        try
        {
            var activity = await _service.NextActivityAsync();
            return this.DefaultOk(activity);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener la siguiente actividad: {e.Message}"); }
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
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al asignar stage {request.StageId} a actividad {request.ActivityId}: {e.Message}"); }
    }

    [Authorize(Roles = "adm")]
    [HttpDelete("Activity/{id}")]
    public async Task<IActionResult> DeleteActivityAsync(
        uint id
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.DeleteActivityAsync(userId, id);
            return this.DefaultOk(new { }, "La actividad se liminó satisfactoriamente");
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al eliminar actividad {id}: {e.Message}"); }
    }

    [HttpDelete("{activityId}/{stageId}")]
    public async Task<IActionResult> DelStageToActivityAsync(
        uint activityId,
        ushort stageId
    )
    {
        try
        {
            await _service.DelStageToActivityAsync(activityId, stageId);
            return this.DefaultOk(new { }, "Se ha removido la fase a la actividad correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al eliminar stage {stageId} en actividad {activityId}: {e.Message}"); }
    }
}