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

    private static bool IsValidPhoneNumber(
        string phone
    )
    {
        if (!long.TryParse(phone, out _)) return false;
        var number = long.Parse(phone);
        return number > 9999999;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] PeopleRequest request
    )
    {
        if (!ModelState.IsValid)
            return this.DefaultBadRequest("Los datos son obligatorios");
        if (string.IsNullOrWhiteSpace(request.Name))
            return this.DefaultBadRequest("El nombre es obligatorio");
        request.Name = request.Name.Trim();
        if (string.IsNullOrWhiteSpace(request.Address))
            return this.DefaultBadRequest("La dirección es obligatoria");
        if (request.Gender == null) return this.DefaultBadRequest("El género es obligatorio");
        if (request.DOB == null) return this.DefaultBadRequest("La fecha de nacimiento es obligatorio");
        if (request.Day == null) return this.DefaultBadRequest("El día es obligatorio");
        if (request.Sacraments != null && request.Sacraments.Count > 10) request.Sacraments = null;
        if (request.DegreeId == null) return this.DefaultBadRequest("El grado académico es obligatorio");
        if (string.IsNullOrWhiteSpace(request.Address)) return this.DefaultBadRequest("La dirección es obligatoria");
        else request.Address = request.Address.Trim();
        if (string.IsNullOrWhiteSpace(request.Phone) || !IsValidPhoneNumber(request.Phone))
            return this.DefaultBadRequest("El número telefónico es requerido");

        if (request.Parents != null)
        {
            request.Parents = request.Parents.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();
            if (request.Parents.Count == 0 || request.Parents.Count > 2) request.Parents = null;
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var id = await _service.CreateAsync(userId, request);
            return this.DefaultCreated(nameof(GetById), id);
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
    public async Task<IActionResult> GetById(
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
        short? degreeId = null,
        string? name = null
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
                DegreeId = degreeId,
                Name = name
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
            if (request.Parents.Count == 0 || request.Parents.Count > 2) request.Parents = null;
        }
        if (request.Godparents != null)
        {
            request.Godparents = request.Godparents.Where(g => !string.IsNullOrWhiteSpace(g.Name)).ToList();
            if (request.Godparents.Count == 0 || request.Godparents.Count > 4) request.Godparents = null;
        }
        if (!string.IsNullOrWhiteSpace(request.Address)) request.Address = request.Address.Trim();
        if (string.IsNullOrWhiteSpace(request.Phone) || !IsValidPhoneNumber(request.Phone)) request.Phone = null;
        if (request.Sacraments != null && request.Sacraments.Count > 10) request.Sacraments = null;
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