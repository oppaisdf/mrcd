using System.Security.Claims;
using api.Common;
using api.Models.Filters;
using api.Models.Requests;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PeopleController(
    IPeopleService service
) : ControllerBase
{
    private readonly IPeopleService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] PeopleRequest request
    )
    {
        if (!ModelState.IsValid)
            return this.DefaultBadRequest("Data is required");
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("Name is required");
        request.Name = request.Name.Trim();

        if (request.Parents != null)
        {
            request.Parents = request.Parents.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();
            if (request.Parents.Count == 0) request.Parents = null;
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var id = await _service.CreateAsync(userId, request);
            return this.DefaultCreated(nameof(GetByIdAsync), id);
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (AlreadyExistsException e)
        { return this.DefaultConflict(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al crear persona: {e.Message}"); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        int id
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var person = await _service.GetByIdAsync(userId, id);
            return this.DefaultOk(person);
        }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al cosultar persona {id}: {e.Message}"); }
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        short page = 1,
        bool? gender = null,
        bool? day = null,
        bool? isActive = null,
        short? degreeId = null
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var filters = new PeopleFilter
            {
                Page = page,
                Gender = gender,
                Day = day,
                IsActive = isActive,
                DegreeId = degreeId
            };
            var (people, counter) = await _service.GetAsync(userId, filters);
            return this.DefaultOk(people, counter);
        }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al consultar personas: {e.Message}"); }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] PeopleRequest request
    )
    {
        if (!string.IsNullOrWhiteSpace(request.Name)) request.Name = request.Name.Trim();
        if (request.Parents != null)
        {
            request.Parents = request.Parents.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();
            if (request.Parents.Count == 0) request.Parents = null;
        }
        if (request.Godparents != null)
        {
            request.Godparents = request.Godparents.Where(g => !string.IsNullOrWhiteSpace(g.Name)).ToList();
            if (request.Godparents.Count == 0) request.Godparents = null;
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.UpdateAsync(userId, id, request);
            return this.DefaultOk(new { }, "Person has been successfully updated!");
        }
        catch (BadRequestException e)
        { return this.DefaultBadRequest(e.Message); }
        catch (DoesNotExistsException e)
        { return this.DefaultNotFound(e.Message); }
        catch (Exception e)
        { return this.DefaultServerError($"[+] Error al actualizar persona {id}: {e.Message}"); }
    }
}