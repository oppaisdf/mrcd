using System.Security.Claims;
using api.Common;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/Planner/[controller]")]
public class StageController(
    IStageService service
) : ControllerBase
{
    private readonly IStageService _service = service;

    [HttpGet()]
    public async Task<IActionResult> GetStagesAsync()
    {
        try
        {
            var stages = await _service.ToListAsync();
            return this.DefaultOk(stages);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener fases de actividades: {e.Message}"); }
    }

    [Authorize(Roles = "adm")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStageAsync(
        ushort id
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.DeleteAsync(userId, id);
            return this.DefaultOk(new { }, "Se eliminó la fase de actividad satisfactoriamente");
        }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al eliminar stage {id}: {e.Message}"); }
    }

    [HttpPost()]
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
            await _service.CreateAsync(userId, request);
            return this.DefaultOk(new { }, "Se ha creado la etapa satisfactoriamente :3");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear etapa {request.Name}: {e.Message}"); }
    }
}