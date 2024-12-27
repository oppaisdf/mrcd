using System.Security.Claims;
using api.Common;
using api.Models.Filters;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttendanceController(
    IAttendanceService service
) : ControllerBase
{
    private readonly IAttendanceService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        short page = 1,
        string? userId = null,
        int? personId = null
    )
    {
        var filters = new AttendanceFilter
        {
            Page = page,
            UserId = userId,
            PersonId = personId
        };

        try
        {
            var _userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var checks = await _service.GetAsync(_userId, filters);
            return this.DefaultOk(checks);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar asistencias: {e.Message}"); }
    }

    [HttpPost("{personId}")]
    public async Task<IActionResult> CheckAsync(
        int personId
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.CheckAsync(userId, personId);
            return this.DefaultOk(new { }, "Se ha registrado la asistencia");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al registar asistencia a {personId}: {e.Message}"); }
    }

    [Authorize(Roles = "amd")]
    [HttpDelete("{attendanceId}")]
    public async Task<IActionResult> Delete(
        int attendanceId
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.RemoveCheckAsync(userId, attendanceId);
            return this.DefaultOk(new { }, "Se ha eliminado la asistecia correctamente");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al eliminar asistenncia {attendanceId}: {e.Message}"); }
    }
}