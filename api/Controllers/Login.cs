using api.Common;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(
    IUserService service
) : ControllerBase
{
    private readonly IUserService _service = service;

    [HttpPost]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request
    )
    {
        if (!ModelState.IsValid)
            return this.DefaultBadRequest("Invalid login data");

        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("Username is required");
        else
            request.Name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(request.Pass))
            return this.DefaultBadRequest("Password is required");
        else
            request.Pass = request.Pass.Trim();

        try
        {
            var roles = await _service.LoginAsync(request);
            return this.DefaultOk(roles, "The cookie has been sent!");
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error en login de usuario {request.Name}: {e.Message}"); }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _service.LogoutAsync();
            return this.DefaultOk(new { }, "Sucessfull logout!");
        }
        catch (Exception e)
        {
            return this.DefaultServerError($"[+] Error al cerrar sesión: ${e.Message}");
        }
    }

    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied()
    {
        return this.DefaultUnauthorized("No, papu, usted no tiene autorización para esta acción. :c");
    }
}