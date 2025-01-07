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
public class ChargeController(
    IChargeService service
) : ControllerBase
{
    private readonly IChargeService _service = service;

    [Authorize(Roles = "adm")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ChargeRequest request
    )
    {
        if (string.IsNullOrEmpty(request.Name)) return this.DefaultBadRequest("El nombre es requerido");
        else request.Name = request.Name.Trim();
        if (request.Total == null || request.Total < 1 || request.Total > 300)
            return this.DefaultBadRequest("El monto del cobro es inválido");
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.CreateAsync(userId, request);
            return this.DefaultOk(new { }, "Se ha creado el cobro correctamente");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"Error al crear cobro: {e.Message}"); }
    }

    [Authorize(Roles = "adm")]
    [HttpPatch("id")]
    public async Task<IActionResult> UpdateAsync(
        short id,
        [FromBody] ChargeRequest request
    )
    {
        if (string.IsNullOrEmpty(request.Name)) request.Name = null;
        if (request.Total == null || request.Total < 1 || request.Total > 300) request.Total = null;
        if (request.Name == null && request.Total == null)
            return this.DefaultBadRequest("No hay información para actualizar");
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UpdateAsync(userId, id, request);
            return this.DefaultOk(new { }, "Se ha actualizado el cobro correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al actualizar cobro {id}: {e.Message}"); }
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var charges = await _service.GetAsync(userId);
            return this.DefaultOk(charges);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener todos los cobros: {e.Message}"); }
    }

    [HttpPost("{id}/{personId}")]
    public async Task<IActionResult> AssingAsync(
        short id,
        int personId
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.AssignAsync(userId, personId, id);
            return this.DefaultOk(new { }, "Se ha registrado el cobro al confirmando correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al asignar cobro {id} a confirmando {personId}: {e.Message}"); }
    }

    [HttpDelete("{id}/{personId}")]
    public async Task<IActionResult> UnassingAsync(
        short id,
        int personId
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UnassignAsync(userId, personId, id);
            return this.DefaultOk(new { }, "Se ha retiado el cobro al confirmando correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al deasignar cobro {id} a confirmando {personId}: {e.Message}"); }
    }
}