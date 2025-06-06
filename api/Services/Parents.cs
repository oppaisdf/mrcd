using api.Common;
using api.Data;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IParentService
{
    /// <summary>
    /// Busca un Parent por nombre, si no existe lo crea.
    /// Luego asigna este Parent al Person.
    /// No se ejecuta en una transacción
    /// </summary>
    /// <param name="userId">Usuario para registro de logs</param>
    /// <param name="personId">Debe existir el Person, no se valida el ID</param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task FindOrCreateAndAssignAsync(string userId, int personId, ParentRequest request);

    /// <summary>
    /// Retorna el ID del parent.
    /// Busca por nombre, si no existe el Parent lo crea.
    /// Se ejecuta en una transacción.
    /// </summary>
    /// <param name="userId">Usuario para logs</param>
    /// <param name="personId">Se valida que el Person exista</param>
    /// <param name="request">El nombre no debe ser nulo</param>
    /// <returns></returns>
    Task<int> GetFindOrCreateAndAssignAsync(string userId, int personId, ParentRequest request);

    /// <summary>
    /// Obtiene todos los Parents asociados a un Person
    /// </summary>
    /// <param name="personId">El Person debe existir, no se valida</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<ICollection<ParentResponse>> GetByPersonIdAsync(int personId);
    Task AssignAsync(string userId, int id, int personId, bool isParent);
    Task UnassignAsync(string userId, int id, int personId);
    Task<(string page, ICollection<ParentResponse> parents)> GetAsync(string userId, ParentFilter filter);
    Task<ParentResponse> GetByIdAsync(string userId, int id);
    Task<int> CreateAsync(string userId, ParentRequest request);
    Task UpdateAsync(string userId, int id, ParentRequest request);
    Task DeleteAsync(string userId, int id);
}

public class ParentService(
    MerContext context,
    ICommonService service,
    ILogService logs
) : IParentService
{
    #region "Private"
    private readonly MerContext _context = context;
    private readonly ICommonService _service = service;
    private readonly ILogService _logs = logs;

    internal class PParentResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Normalized { get; set; }
        public required bool Gender { get; set; }
        public required bool HasChild { get; set; }
        public required bool HasGodchild { get; set; }

    }

    /// <summary>
    /// Crea el Parent, pero debe estar validado que no exista otro con el mismo hash
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="hash"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<Parent> PCreateAsync(
        string userId,
        string hash,
        ParentRequest request
    )
    {
        var parent = new Parent
        {
            Name = request.Name!,
            NameHash = hash,
            Gender = request.Gender == null || request.Gender!.Value,
            Phone = request.Phone
        };
        _context.Parents.Add(parent);
        await _context.SaveChangesAsync();
        await _logs.RegisterCreationAsync(userId, $"Parent {parent.Id}");
        return parent;
    }

    private async Task<int> PFindOrCeateAndAssignAsync(
        string userId,
        ParentRequest request,
        int personId
    )
    {
        var hash = _service.GetHashedString(request.Name!);
        var parent =
            await _context.Parents.Where(p => p.NameHash == hash).FirstOrDefaultAsync() ??
            await PCreateAsync(userId, hash, request);

        _context.ParentsPeople.Add(new ParentPerson
        {
            PersonId = personId,
            ParentId = parent.Id!.Value,
            IsParent = request.IsParent == null || request.IsParent!.Value
        });

        await _context.SaveChangesAsync();
        await _logs.RegisterCreationAsync(userId, $"Person/Parent {personId}/{parent.Id}");
        return parent.Id!.Value;
    }
    #endregion

    public async Task FindOrCreateAndAssignAsync(
        string userId,
        int personId,
        ParentRequest request
    ) => await PFindOrCeateAndAssignAsync(userId, request, personId);

    public async Task<int> GetFindOrCreateAndAssignAsync(
        string userId,
        int personId,
        ParentRequest request
    )
    {
        var exists = await _context.People.AnyAsync(p => p.Id == personId && p.IsActive);
        if (!exists) throw new DoesNotExistsException("El confirmando no existe o está inactivo");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            var id = await PFindOrCeateAndAssignAsync(userId, request, personId);
            await tran.CommitAsync();
            return id;
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<ICollection<ParentResponse>> GetByPersonIdAsync(
        int personId
    )
    {
        return await (
            from p in _context.Parents
            join pp in _context.ParentsPeople on p.Id equals pp.ParentId
            where pp.PersonId == personId
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

    public async Task<ParentResponse> GetByIdAsync(
        string userId,
        int id
    )
    {
        await _logs.RegisterReadingAsync(userId, $"Parent {id}");
        return await _context.Parents
            .Where(p => p.Id == id)
            .Select(p => new ParentResponse
            {
                Id = p.Id!.Value,
                Name = p.Name,
                Gender = p.Gender,
                Phone = p.Phone,
                IsParent = false,
                People = (
                    from person in _context.People
                    join temp in _context.ParentsPeople on person.Id equals temp.PersonId into tempG
                    from pp in tempG.DefaultIfEmpty()
                    where person.IsActive || pp != null
                    group pp by new
                    {
                        Id = person.Id!.Value,
                        person.Name,
                        person.Day,
                        person.Gender,
                        person.IsActive
                    } into r
                    select new BasicPersonResponse
                    {
                        Id = r.Key.Id,
                        Name = r.Key.Name,
                        Gender = r.Key.Gender,
                        Day = r.Key.Day,
                        IsActive = r.Key.IsActive,
                        HasParent = r
                            .Where(x => x.ParentId == p.Id)
                            .Select(x => (bool?)x.IsParent)
                            .FirstOrDefault()
                    }
                ).ToList()
            })
            .FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El padre/padrino no existe");
    }

    public async Task<(string page, ICollection<ParentResponse> parents)> GetAsync(
        string userId,
        ParentFilter filter
    )
    {
        await _logs.RegisterReadingAsync(userId, "Todos los Parents");
        var query =
            from p in _context.Parents
            join temp in _context.ParentsPeople on p.Id equals temp.ParentId into tempG
            from pp in tempG.DefaultIfEmpty()
            group pp by new
            {
                Id = p.Id!.Value,
                p.Name,
                p.Gender
            } into r
            select new PParentResponse
            {
                Id = r.Key.Id,
                Gender = r.Key.Gender,
                Name = r.Key.Name,
                Normalized = "",
                HasChild = r.Any(x => x.IsParent),
                HasGodchild = r.Any(x => !x.IsParent)
            };

        if (filter.Gender != null) query = query.Where(p => p.Gender == filter.Gender);
        if (filter.IsParent != null)
            if (filter.IsParent!.Value) query = query.Where(p => p.HasChild);
            else query = query.Where(p => p.HasGodchild);

        short pages;
        ICollection<ParentResponse> parents;
        if (filter.Name == null)
        {
            pages = (short)Math.Ceiling(await query.CountAsync() / 15.0);
            if (filter.Page > pages) filter.Page = pages;
            if (pages == 0) parents = [];
            else
                parents = await query
                    .Skip((filter.Page - 1) * 15)
                    .Take(15)
                    .Select(p => new ParentResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Gender = p.Gender,
                        IsParent = false
                    })
                    .ToListAsync();
        }
        else
        {
            var pp = await query.AsNoTracking().ToListAsync();
            pp.ForEach(p => p.Normalized = _service.GetNormalizedText(p.Name));
            var normalized = _service.GetNormalizedText(filter.Name);
            pp = pp.Where(p => p.Name.Contains(normalized)).ToList();
            pages = (short)Math.Ceiling(pp.Count / 15.0);
            if (filter.Page > pages) filter.Page = pages;
            if (pages == 0) parents = [];
            else parents = pp
                .Skip((filter.Page - 1) * 15)
                .Take(15)
                .Select(p => new ParentResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Gender = p.Gender,
                    IsParent = false
                })
                .ToList();
        }

        return ($"{filter.Page}/{pages}", parents);
    }

    public async Task AssignAsync(
        string userId,
        int id,
        int personId,
        bool isParent
    )
    {
        var exists = await _context.Parents.AnyAsync(p => p.Id == id);
        if (!exists) throw new DoesNotExistsException("El padre/padrino no existe");
        exists = await _context.People.AnyAsync(p => p.Id == personId);
        if (!exists) throw new DoesNotExistsException("El confirmando no existe");
        exists = await _context.ParentsPeople.AnyAsync(p => p.ParentId == id && p.PersonId == personId && p.IsParent == isParent);
        if (exists) throw new AlreadyExistsException($"Ya se ha asignado el {(isParent ? "padre" : "padrino")} al confirmando");

        _context.ParentsPeople.Add(new ParentPerson
        {
            ParentId = id,
            PersonId = personId,
            IsParent = isParent
        });

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SaveChangesAsync();
            await _logs.RegisterCreationAsync(userId, $"Parent/Person {id}/{personId}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task UnassignAsync(
        string userId,
        int id,
        int personId
    )
    {
        var parentPerson = await _context.ParentsPeople
            .Where(pp => pp.ParentId == id && pp.PersonId == personId)
            .FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El padre/padrino o el confirmando no existe");
        _context.ParentsPeople.Remove(parentPerson);

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SaveChangesAsync();
            await _logs.RegisterUpdateAsync(userId, $"Parent/Person {id}/{personId}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<int> CreateAsync(
        string userId,
        ParentRequest request
    )
    {
        var hash = _service.GetHashedString(request.Name!);
        var alreadyExists = await _context.Parents.AnyAsync(p => p.NameHash == hash);
        if (alreadyExists) throw new AlreadyExistsException("El padre/padrino ya existe");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            var parent = await PCreateAsync(userId, hash, request);
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
        if (request.Gender != null && request.Gender != parent.Gender) parent.Gender = request.Gender!.Value;
        if (request.Phone != null) parent.Phone = request.Phone;
        if (request.Name != null)
        {
            var hashed = _service.GetHashedString(request.Name);
            if (hashed != parent.NameHash)
            {
                var alreadyExists = await _context.Parents.AnyAsync(p => p.NameHash == hashed && p.Id != id);
                if (alreadyExists) throw new AlreadyExistsException("El padre/padrino ya existe");
                parent.Name = request.Name;
                parent.NameHash = hashed;
            }
        }

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SaveChangesAsync();
            await _logs.RegisterUpdateAsync(userId, $"Padre/padrino {id}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(
        string userId,
        int id
    )
    {
        var parent = await _context.Parents.FindAsync(id) ?? throw new DoesNotExistsException("El padre/padrino no existe");
        var hasChildrens = await _context.ParentsPeople.AnyAsync(p => p.ParentId == id);
        if (hasChildrens) throw new BadRequestException("No se puede eliminar el padre/padrino porque tiene hijos/ahijados asociados");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            await _logs.RegisterUpdateAsync(userId, $"Borró padre/padrino {id}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}