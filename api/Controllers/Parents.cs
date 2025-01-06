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

    [HttpPost("{id}")]
    public async Task<IActionResult> AssignAsync(
        int id,
        [FromBody] ICollection<AssignParentRequest> request
    )
    {
        if (request == null || request.Count == 0)
            return this.DefaultBadRequest("Los padres/padrinos son requerios");
        else if (request.Count > 5)
            return this.DefaultBadRequest("No puede asignar tantos padres/padrinos");
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.AssignAsync(userId, id, request);
            return this.DefaultOk(new { }, "Se han asignado los padres/padrinos correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar los padres: {e.Message}"); }
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
            await _service.UnassignAsync(userId, id, parentId);
            return this.DefaultOk(new { }, "Se han desasignado los padres/padrinos correctamente");
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al desasingar padre/padrino de confirmando: {e.Message}"); }
    }
}