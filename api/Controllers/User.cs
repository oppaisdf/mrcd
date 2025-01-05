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
public class UserController(
    IUserService service
) : ControllerBase
{
    private readonly IUserService _service = service;

    [Authorize(Roles = "adm")]
    [HttpPost]
    public async Task<ActionResult> CreateAsync(
        [FromBody] UserRequest request
    )
    {
        if (!ModelState.IsValid)
            return this.DefaultBadRequest("Información inválida");
        if (string.IsNullOrWhiteSpace(request.Username))
            return this.DefaultBadRequest("El usuario es requerido");
        else
            request.Username = request.Username.Trim();

        if (string.IsNullOrWhiteSpace(request.Password))
            return this.DefaultBadRequest("La contraseña es requerida");
        else
            request.Password = request.Password.Trim();

        if (request.Roles == null || request.Roles.Count == 0)
            return this.DefaultBadRequest("Los roles son requeridos");

        try
        {
            var user = await _service.CreateAsync(request);
            return this.DefaultOk(new { user.Id }, "El usuario ha sido creado correctamente");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear usuario: {e.Message}"); }
    }

    [Authorize(Roles = "adm")]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var users = await _service.GetAsync();
            return this.DefaultOk(users);
        }
        catch (Exception e)
        {
            return this.DefaultServerError($"[+] Error al consultar los usuarios: ${e.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        string id
    )
    {
        var myRoles = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();
        if (string.IsNullOrWhiteSpace(id) || !myRoles.Contains("adm")) id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        try
        {
            var user = await _service.GetByIdAsync(id);
            return this.DefaultOk(user);
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        {
            return this.DefaultServerError($"[+] Error al desactivar al usuario {id}: {e.Message}");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(
        string id,
        [FromBody] UserRequest request
    )
    {
        if (!ModelState.IsValid)
            return this.DefaultBadRequest("Datos inválidos");
        if (string.IsNullOrWhiteSpace(request.Username)) request.Username = null;
        else request.Username = request.Username.Trim();
        if (string.IsNullOrWhiteSpace(request.Password)) request.Password = null;
        else request.Password = request.Password.Trim();

        var _userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var myRoles = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();

        if (myRoles.Contains("sys"))
        {
            request.Roles = null;
            request.IsActive = null;
        }

        if (_userId != id && !myRoles.Contains("adm"))
        {
            id = _userId;
            request.Roles = null;
            request.IsActive = null;
        }

        request.Roles ??= ([]);
        if (request.Username == null && request.Password == null && request.Roles.Count == 0 && request.IsActive == null)
            return this.DefaultBadRequest("No se encontró información para actualizar");

        try
        {
            await _service.UpdateAsync(id, request, _userId);
            return this.DefaultOk(new { }, "El usuario se actualizó correctamente :3");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        {
            return this.DefaultServerError($"Error al actualizar usuario {id}: {e.Message}");
        }
    }
}