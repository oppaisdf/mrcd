using System.Security.Claims;
using api.Common;
using api.Models.Entities;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SacramentController(
    INameService<Sacrament> service
) : ControllerBase
{
    private readonly INameService<Sacrament> _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var sacraments = await _service.GetAsync(userId);
            return this.DefaultOk(sacraments);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar sacramentos: {e.Message}"); }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] NameRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("El nombre es requerido");
        request.Name = request.Name.Trim();
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.CreateAsync(userId, request.Name);
            return this.DefaultOk(new { }, "Sacramento creado correctamente");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear sacramento: {e.Message}"); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        short id,
        [FromBody] NameRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("El nombre es requerido");
        request.Name = request.Name;

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UpdateAsync(userId, id, request.Name);
            return this.DefaultOk(new { }, "El sacramento fue actualizado correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al actualizar sacramento {id}: {e.Message}"); }
    }
}