using System.Security.Claims;
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

    [HttpPost]
    public async Task<IActionResult> Get(
        [FromBody] ParentRequest request
    )
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var x = await _service.CreateAsync(userId, request);
            return this.DefaultOk(x);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar los padres: {e.Message}"); }
    }
}