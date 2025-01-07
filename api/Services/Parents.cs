using api.Common;
using api.Context;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IParentService
{
    Task<int> CreateAndAssignAsync(string userId, int id, ParentRequest request);
    Task<int> CreateAsync(string userId, ParentRequest request);
    Task UpdateAsync(string userId, int id, ParentRequest request);
    Task DeleteAsync(string userId, int id);
    Task AssignAsync(string userId, int id, ICollection<AssignParentRequest> parents);
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

    public async Task AssignAsync(
        string userId,
        int id,
        ICollection<AssignParentRequest> parents
    )
    {
        var ids = parents.Select(p => p.Id).Distinct().ToList();
        var cleanIds = await _context.Parents
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id!.Value))
            .Select(p => p.Id)
            .ToListAsync() ?? throw new DoesNotExistsException("Los padres/padrinos no existen");

        foreach (var parent in parents)
        {
            _context.ParentsPeople.Add(new ParentPerson
            {
                PersonId = id,
                ParentId = parent.Id,
                IsParent = parent.IsParent
            });
        }

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
        await _context.SaveChangesAsync();
        return parent.Id!.Value;
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
            await _context.SaveChangesAsync();
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<int> CreateAndAssignAsync(
        string userId,
        int id,
        ParentRequest request
    )
    {
        var isValid = await (
            from p in _context.People
            join temp in _context.ParentsPeople on p.Id equals temp.PersonId into tempG
            from pp in tempG.DefaultIfEmpty()
            where p.Id == id
            group new { pp } by new { p.IsActive } into r
            select new
            {
                r.Key.IsActive,
                Parents = r.Where(x => x.pp.IsParent == request.IsParent!.Value).Count()
            }
        ).FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El confirmando no existe");

        var max = request.IsParent!.Value ? 2 : 3;
        if (!isValid.IsActive || isValid.Parents == max) throw new BadRequestException($"El confirmando ya tiene {max} padres/padrinos asignados");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            var parentId = await GetIdByNameAsync(request.Name!) ?? await CreateAsync(userId, request);
            await _context.SaveChangesAsync();

            var pp = new ParentPerson
            {
                PersonId = id,
                ParentId = parentId,
                IsParent = request.IsParent!.Value
            };
            _context.ParentsPeople.Add(pp);
            await _context.SaveChangesAsync();
            await tran.CommitAsync();
            return parentId;
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}