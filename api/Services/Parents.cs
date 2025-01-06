using api.Common;
using api.Context;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IParentService
{
    Task<int> CreateAsync(string userId, ParentRequest request);
    Task UpdateAsync(string userId, int id, ParentRequest request);
    Task DeleteAsync(string userId, int id);
    Task UnassignAsync(string userId, int personId, int parentId);
    Task<int?> GetIdByNameAsync(string name);
    Task<ICollection<ParentResponse>> GetByPersonId(int id);
}

public class ParentService(
    MerContext context,
    ICommonService service
) : IParentService
{
    private readonly MerContext _context = context;
    private readonly ICommonService _service = service;

    #region "Private"
    private async Task CreatePersonParentAsync(
        ICollection<PersonParentRequest> request
    )
    {
        var peopleId = request.Select(r => r.PersonId).Distinct().ToList();
        var parentsId = request.Select(r => r.ParentId).Distinct().ToList();

        var people = await _context.People
            .AsNoTracking()
            .Where(p => peopleId.Contains(p.Id!.Value) && p.IsActive == true)
            .Select(p => p.Id!.Value)
            .ToListAsync();
        var parents = await _context.Parents
            .AsNoTracking()
            .Where(p => parentsId.Contains(p.Id!.Value))
            .Select(p => p.Id!.Value)
            .ToListAsync();

        foreach (var pp in request)
        {
            if (!people.Contains(pp.PersonId) || !parents.Contains(pp.ParentId)) continue;
            _context.ParentsPeople.Add(new ParentPerson
            {
                ParentId = pp.ParentId,
                PersonId = pp.PersonId,
                IsParent = pp.IsParent
            });
        }

        await _context.SaveChangesAsync();
    }
    #endregion

    public async Task<int?> GetIdByNameAsync(
        string name
    )
    {
        var normalized = _service.GetHashedString(name);
        return await _context.Parents
            .Where(p => p.NameHash == normalized)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(
        string userId,
        int id
    )
    {
        var parent = await (
            from p in _context.Parents
            join temp in _context.ParentsPeople on p.Id equals temp.ParentId into ppG
            from pp in ppG.DefaultIfEmpty()
            where p.Id == id
            group new { pp } by new { p } into pR
            select new
            {
                Parent = pR.Key.p,
                People = pR.Where(x => x.pp != null).Count()
            }
        ).FirstOrDefaultAsync() ?? throw new DoesNotExistsException("No existe este padre/padrino");

        if (parent.People > 0) throw new BadRequestException("No se puede eliminar este padre/padrino porque está asociado a confirmandos");
        _context.Parents.Remove(parent.Parent);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<ParentResponse>> GetByPersonId(
        int id
    )
    {
        return await (
            from p in _context.Parents
            join pp in _context.ParentsPeople on p.Id equals pp.ParentId
            where pp.PersonId == id
            select new ParentResponse
            {
                Id = p.Id!.Value,
                Name = p.Name,
                Gender = p.Gender,
                Phone = p.Phone,
                IsParent = pp.IsParent
            }
        ).ToListAsync();
    }

    public async Task UnassignAsync(
        string userId,
        int personId,
        int parentId
    )
    {
        var lst = await _context.ParentsPeople
            .Where(p => p.PersonId == personId && p.ParentId == parentId)
            .ToListAsync();
        if (lst.Count == 0) throw new DoesNotExistsException("No hay datos para remover");
        _context.ParentsPeople.RemoveRange(lst);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(
        string userId,
        ParentRequest request
    )
    {
        var hash = _service.GetHashedString(request.Name!);
        var alreadyExits = await _context.Parents.AnyAsync(p => p.NameHash == hash);
        if (alreadyExits) throw new BadHttpRequestException("El padre/padrino ya existe");
        var parent = new Parent
        {
            Name = request.Name!,
            NameHash = hash,
            Gender = request.Gender!.Value,
            Phone = request.Phone
        };
        _context.Parents.Add(parent);

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SaveChangesAsync();
            if (request.People != null) await CreatePersonParentAsync(request.People);
            await tran.CommitAsync();
            return parent.Id!.Value;
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(
        string userId,
        int id,
        ParentRequest request
    )
    {
        var parent = await _context.Parents.FindAsync(id) ?? throw new DoesNotExistsException("El padre/padrino no existe");
        if (request.Name != null && parent.Name != request.Name)
        {
            var hash = _service.GetHashedString(request.Name);
            var alreadyExits = await _context.Parents.AnyAsync(p => p.NameHash == hash);
            if (alreadyExits) throw new BadRequestException("El padre/padrino ya existe");
            parent.NameHash = hash;
            parent.Name = request.Name;
        }

        if (request.Gender != null && request.Gender != parent.Gender) parent.Gender = request.Gender!.Value;
        if (request.Phone != null && request.Phone != parent.Phone) parent.Phone = request.Phone;

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            if (request.People != null) await CreatePersonParentAsync(request.People);
            await _context.SaveChangesAsync();
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}