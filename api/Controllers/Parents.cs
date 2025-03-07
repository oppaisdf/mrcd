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
public class ParentController(
    IParentService service
) : ControllerBase
{
    private readonly IParentService _service = service;

    private static bool IsValidPhoneNumber(
        string phone
    )
    {
        if (!long.TryParse(phone, out _)) return false;
        var number = long.Parse(phone);
        return number > 9999999;
    }

    [HttpPost("{id}/{personId}")]
    public async Task<IActionResult> AssignAsync(
        int id,
        int personId,
        [FromBody] bool isParent = true
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.AssignAsync(userId, id, personId, isParent);
            return this.DefaultOk(new { }, $"Se ha asignado correctamente el {(isParent ? "encargado" : "padrino")} al confirmando");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al asignar padre/padrino {id} a confirmando {personId}: {e.Message}"); }
    }

    [HttpDelete("{id}/{parentId}")]
    public async Task<IActionResult> UnassignAsync(
        int id,
        int parentId
    )
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        try
        {
            await _service.UnassignAsync(userId, parentId, id);
            return this.DefaultOk(new { }, "Se han desasignado los padres/padrinos correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al desasingar padre/padrino de confirmando: {e.Message}"); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var parent = await _service.GetByIdAsync(userId, id);
            return this.DefaultOk(parent);
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar Parent {id}: {e.Message}"); }
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        short page = 1,
        bool? gender = null,
        bool? isParent = null,
        string? name = null
    )
    {
        if (string.IsNullOrWhiteSpace(name)) name = null;
        else if (name.Length > 30) name = null;
        if (page < 1) page = 1;
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var filters = new ParentFilter
            {
                Page = page,
                Gender = gender,
                IsParent = isParent,
                Name = name
            };
            var (pages, parents) = await _service.GetAsync(userId, filters);
            return this.DefaultOk(parents, pages);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener todos los Parents: {e.Message}"); }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ParentRequest request
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var id = await _service.CreateAsync(userId, request);
            return this.DefaultCreated(nameof(GetById), id);
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear Parent: {e.Message}"); }
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> CreateAndAssignAsync(
        int id,
        [FromBody] ParentRequest request
    )
    {
        if (string.IsNullOrEmpty(request.Name)) return this.DefaultBadRequest("El nombre es requerido");
        if (request.Gender == null) return this.DefaultBadRequest("El género es requerido");
        if (request.IsParent == null) return this.DefaultBadRequest("Debe especificar si es padre/padrino");
        if (string.IsNullOrEmpty(request.Phone) || !IsValidPhoneNumber(request.Phone))
            request.Phone = null;

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var parentId = await _service.GetFindOrCreateAndAssignAsync(userId, id, request);
            return this.DefaultOk(new { Id = parentId }, "Se agregó el padre/padrino correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear padre/padrino: {e.Message}"); }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] ParentRequest request
    )
    {
        if (string.IsNullOrEmpty(request.Name)) request.Name = null;
        else request.Name = request.Name.Trim();
        if (string.IsNullOrEmpty(request.Phone) || !IsValidPhoneNumber(request.Phone)) request.Phone = null;
        if (request.Name == null && request.Phone == null && request.Gender == null)
            return this.DefaultBadRequest("No se encontró información para actualizar");
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UpdateAsync(userId, id, request);
            return this.DefaultOk(new { }, "Se ha actualizado el padre/padrino correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al actualizar padre {id}: {e.Message}"); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        int id
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.DeleteAsync(userId, id);
            return this.DefaultOk(new { }, "Se ha eliminado el padre/padrino correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al eliminar padre {id}: {e.Message}"); }
    }
}