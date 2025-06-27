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
public class AttendanceController(
    IAttendanceService service
) : ControllerBase
{
    private readonly IAttendanceService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var attendances = await _service.GetAsync(userId);
            return this.DefaultOk(attendances);
        }
        catch (Exception e)
        { return this.DefaultServerError($"Error al consultar las asistencias: {e.Message}"); }
    }

    [HttpGet("List")]
    public async Task<IActionResult> GetListAync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var list = await _service.GetListAsync(userId);
            return this.DefaultOk(list);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener listado general: {e.Message}"); }
    }

    [HttpPost]
    public async Task<IActionResult> CheckAsync(
        [FromBody] AttendanceRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Hash))
            return this.DefaultBadRequest("El hash es requerido");
        if (request.Date != null)
        {
            TimeSpan diff = DateTime.UtcNow.AddHours(-6) - request.Date!.Value;
            if (diff.Days > 15 || diff.Days < -10) return this.DefaultBadRequest("La fecha no puede exceder los 15 días ni puede registrarse 10 días atrás a la fecha actual (hoy)");
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var _request = request with
            {
                Hash = request.Hash,
                IsAttendance = request.IsAttendance == null ? true : request.IsAttendance
            };
            await _service.CheckAsync(userId, _request);
            return this.DefaultOk(new { }, "Se ha registrado la asistencia");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al registar asistencia a {request.Hash}: {e.Message}"); }
    }

    [HttpPost("All")]
    public async Task<IActionResult> CheckAllAsync(
        bool day = true
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.CheckAllAsync(userId, day);
            return this.DefaultOk(new { }, "Se han registrado las asistencias correctamente");
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al registar asistencias al día {day}: {e.Message}"); }
    }

    [HttpDelete("{hash}")]
    public async Task<IActionResult> UncheckAsync(
        string hash
    )
    {
        if (string.IsNullOrWhiteSpace(hash))
            return this.DefaultBadRequest("El hash es requerido");
        hash = hash.Trim();
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UnverifyAsync(userId, hash);
            return this.DefaultOk(new { }, "Se ha removido la última asistencia");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al registar asistencia a {hash}: {e.Message}"); }
    }

    [HttpGet("QR")]
    public async Task<IActionResult> GetQRsAsync()
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var qrs = await _service.GetQRsAsync(userId);
            return this.DefaultOk(qrs);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar QRs: {e.Message}"); }
    }
}