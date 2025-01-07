using api.Common;
using api.Context;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IChargeService
{
    Task CreateAsync(string userId, ChargeRequest request);
    Task UpdateAsync(string userId, short id, ChargeRequest request);
    Task<ICollection<ChargeResponse>> GetAsync(string userId);
    Task AssignAsync(string userId, int personId, short chargeId);
    Task UnassignAsync(string userId, int personId, short chargeId);
}

public class ChargeService(
    MerContext context,
    ICommonService service,
    ILogService logs
) : IChargeService
{
    private readonly MerContext _context = context;
    private readonly ICommonService _service = service;
    private readonly ILogService _logs = logs;

    #region "Private"
    private async Task<bool> AlreadyExistsAsync(
        string name
    )
    {
        var normalized = _service.GetNormalizedText(name);
        var charges = await _context.Charges
            .AsNoTracking()
            .Select(c => c.Name)
            .ToListAsync();
        charges.ForEach(c => c = _service.GetNormalizedText(c));
        return charges.Contains(normalized);
    }
    #endregion

    public async Task AssignAsync(
        string userId,
        int personId,
        short chargeId
    )
    {
        var exists = await _context.People.AnyAsync(p => p.IsActive && p.Id == personId);
        if (!exists) throw new DoesNotExistsException("El confirmando no existe o está inactivo");
        var charge = await _context.Charges.FindAsync(chargeId) ?? throw new DoesNotExistsException("El cobro no existe");

        _context.PeopleCharges.Add(new PersonCharge
        {
            PersonId = personId,
            ChargeId = chargeId,
            Total = charge.Total
        });
        await _context.SaveChangesAsync();
        await _logs.RegisterCreationAsync(userId, $"Cobro/Confirmando({chargeId}/{personId})");
    }

    public async Task UnassignAsync(
        string userId,
        int personId,
        short chargeId
    )
    {
        var exits = await _context.People.AnyAsync(p => p.IsActive && p.Id == personId);
        if (!exits) throw new DoesNotExistsException("El confirmando no existe o está inactivo");
        var chargePerson = await _context.PeopleCharges.Where(pc => pc.ChargeId == chargeId && pc.PersonId == personId).FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El cobro no existen");
        _context.PeopleCharges.Remove(chargePerson);
        await _context.SaveChangesAsync();
        await _logs.RegisterUpdateAsync(userId, $"Cobro/Confirmando({chargeId}/{personId})");
    }

    public async Task CreateAsync(
        string userId,
        ChargeRequest request
    )
    {
        var alreadyExists = await AlreadyExistsAsync(request.Name!);
        if (alreadyExists) throw new AlreadyExistsException("El cobro ya existe");
        var charge = new Charge
        {
            Name = request.Name!,
            Total = request.Total!.Value
        };
        _context.Charges.Add(charge);
        await _context.SaveChangesAsync();
        await _logs.RegisterCreationAsync(userId, $"Cobro {charge.Id}");
    }

    public async Task<ICollection<ChargeResponse>> GetAsync(
        string userId
    )
    {
        return await _context.Charges
            .AsNoTracking()
            .Select(c => new ChargeResponse
            {
                Id = c.Id!.Value,
                Name = c.Name,
                Total = c.Total
            })
            .ToListAsync();
    }

    public async Task UpdateAsync(
        string userId,
        short id,
        ChargeRequest request
    )
    {
        var charge = await _context.Charges.FindAsync(id) ?? throw new DoesNotExistsException("El cobro no existe");
        if (request.Name != null && request.Name != charge.Name)
        {
            var alreadyExists = await AlreadyExistsAsync(request.Name);
            if (alreadyExists) throw new AlreadyExistsException("El cobro ya existe");
            charge.Name = request.Name;
        }

        if (request.Total != null && request.Total != charge.Total) charge.Total = request.Total!.Value;

        await _context.SaveChangesAsync();
        await _logs.RegisterUpdateAsync(userId, $"Cobro {id}");
    }
}