using api.Common;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize(Roles = "adm")]
[ApiController]
[Route("api/[controller]")]
public class UserController(
    IUserService service
) : ControllerBase
{
    private readonly IUserService _service = service;

    #region "Login"
    [AllowAnonymous]
    [HttpPost("Login")]
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
        catch (Common.DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error en login de usuario {request.Name}: {e.Message}"); }
    }

    [Authorize]
    [HttpDelete("Logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        try
        {
            await _service.LogoutAsync();
            return this.DefaultOk(new { }, "Sucessful logout!");
        }
        catch (Exception e)
        {
            return this.DefaultServerError($"[+] Error al cerrar sesión: ${e.Message}");
        }
    }

    [AllowAnonymous]
    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied()
    {
        return this.DefaultUnauthorized("No, papu, usted no tiene autorización para esta acción. :c");
    }
    #endregion

    [HttpPost]
    public async Task<ActionResult> CreateAsync(
        [FromBody] UserRequest request
    )
    {
        if (!ModelState.IsValid)
            return this.DefaultBadRequest("Invalid User data");
        if (string.IsNullOrWhiteSpace(request.Username))
            return this.DefaultBadRequest("Username is required");
        request.Username = request.Username.Trim();
        if (request.Username == "")
            return this.DefaultBadRequest("Username is required");

        if (string.IsNullOrWhiteSpace(request.Password))
            return this.DefaultBadRequest("Password is required");
        request.Password = request.Password.Trim();
        if (request.Password == "")
            return this.DefaultBadRequest("Password is required");

        if (string.IsNullOrWhiteSpace(request.Email))
            request.Email = "fake@fake.com";

        if (request.Roles == null || request.Roles.Count == 0)
            return this.DefaultBadRequest("Roles is required");

        try
        {
            var user = await _service.CreateAsync(request);
            return this.DefaultOk(new { user.Id }, "User has been created!");
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
        if (string.IsNullOrWhiteSpace(id))
            return this.DefaultBadRequest("Invalid User Id");

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
            return this.DefaultBadRequest("Invalid user data");

        request.Roles ??= ([]);
        if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Password) && request.Roles.Count == 0)
            return this.DefaultBadRequest("No data found to update");

        try
        {
            var _userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            await _service.UpdateAsync(id, request, _userId!);
            return this.DefaultOk(new { }, "User has been updated :3");
        }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (Exception e)
        {
            return this.DefaultServerError($"Error al actualizar usuario {id}: {e.Message}");
        }
    }
}