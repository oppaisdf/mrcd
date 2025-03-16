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
public class DocumentController(
    INameService<Document> defaultService,
    IDocumentService service
) : ControllerBase
{
    private readonly INameService<Document> _defaultService = defaultService;
    private readonly IDocumentService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var sacraments = await _defaultService.GetAsync(userId);
            return this.DefaultOk(sacraments);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar documentos: {e.Message}"); }
    }

    [Authorize(Roles = "adm")]
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
            await _defaultService.CreateAsync(userId, request.Name);
            return this.DefaultOk(new { }, "Documento creado correctamente");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear documento: {e.Message}"); }
    }

    [Authorize(Roles = "adm")]
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
            await _defaultService.UpdateAsync(userId, id, request.Name);
            return this.DefaultOk(new { }, "El documento fue actualizado correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al actualizar documento {id}: {e.Message}"); }
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> AssingAsync(
        short id,
        int personId
    )
    {
        if (personId < 1) return this.DefaultBadRequest("El Id del confirmando es requerido");
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.AssignAsync(userId, id, personId);
            return this.DefaultOk(new { }, "Se ha registrado la recepción del documento correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al asignar documento {id} a persona {personId}: {e.Message}"); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> UnassingAsync(
        short id,
        int personId
    )
    {
        if (personId < 1) return this.DefaultBadRequest("El Id del confirmando es requerido");
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UnassignAsync(userId, id, personId);
            return this.DefaultOk(new { }, "Se ha eliminado entrega de documento correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al desasignar documento {id} a persona {personId}: {e.Message}"); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        short id
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var document = await _service.FindBydIdAsync(userId, id);
            return this.DefaultOk(document);
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener documento {id}: {e.Message}"); }
    }
}