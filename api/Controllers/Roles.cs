using api.Common;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize(Roles = "sys")]
[ApiController]
[Route("api/[controller]")]
public class RoleController(
    IRoleService service
) : ControllerBase
{
    private readonly IRoleService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var roles = await _service.GetAsync();
            return this.DefaultOk(roles);
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al obtener roles: {e.Message}"); }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] NameRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("Name is required");
        request.Name = request.Name.Trim();
        try
        {
            await _service.CreateAsync(request.Name);
            return this.DefaultOk(new { }, "Role has been successfully created");
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear rol {request.Name}: {e.Message}"); }
    }
}