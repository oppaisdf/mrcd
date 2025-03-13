using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IAlertRepository
{
    Task<ushort> NoPaymentCountAsync();
    Task<ushort> NoGodparentsCountAsync();
    Task<ushort> NoChildsCountAsync();
    Task<ushort> NoDocumentsCountAsync();
    Task<IEnumerable<PersonResponse>> NoDocumentsAsync();
    Task<IEnumerable<PersonResponse>> NoPaymentAsync();
    Task<IEnumerable<PersonResponse>> NoGodparentsAsync();
    Task<IEnumerable<ParentResponse>> NoChildsAsync();
}

public class AlertRepository(
    MerContext context
) : IAlertRepository
{
    private readonly MerContext _context = context;

    #region "Counters"
    public async Task<ushort> NoChildsCountAsync()
    {
        return (ushort)await _context.Parents
            .GroupJoin(
                _context.ParentsPeople,
                p => p.Id,
                pp => pp.ParentId,
                (p, pp) => new
                {
                    HasChilds = pp.Any()
                }
            )
            .Where(x => !x.HasChilds)
            .CountAsync();
    }

    public async Task<ushort> NoPaymentCountAsync()
    {
        var totalCharges = await _context.Charges.CountAsync();
        return (ushort)await _context.People
            .GroupJoin(
                _context.PeopleCharges,
                p => p.Id,
                pc => pc.PersonId,
                (p, pc) => new
                {
                    Person = p,
                    PersonChargesCount = pc.Count()
                }
            )
            .Where(x => x.Person.IsActive && x.PersonChargesCount < totalCharges)
            .CountAsync();
    }

    public async Task<ushort> NoGodparentsCountAsync()
    {
        return (ushort)await _context.People
            .GroupJoin(
                _context.ParentsPeople,
                p => p.Id,
                pp => pp.PersonId,
                (p, pp) => new
                {
                    Person = p,
                    HasGodparents = pp.Any(x => !x.IsParent)
                }
            )
            .Where(p => p.Person.IsActive && !p.HasGodparents)
            .CountAsync();
    }

    public async Task<ushort> NoDocumentsCountAsync()
    {
        var documentsCount = await _context.Documents.CountAsync();
        return (ushort)await _context.People
            .GroupJoin(
                _context.PeopleDocuments,
                p => p.Id,
                pd => pd.PersonId,
                (p, pd) => new
                {
                    Person = p,
                    DocumentsCount = pd.Count()
                }
            )
            .Where(g => g.Person.IsActive && g.DocumentsCount < documentsCount)
            .CountAsync();
    }
    #endregion

    public async Task<IEnumerable<ParentResponse>> NoChildsAsync()
    {
        return await _context.Parents
            .AsNoTracking()
            .GroupJoin(
                _context.ParentsPeople,
                p => p.Id,
                pp => pp.ParentId,
                (p, pp) => new
                {
                    Parent = p,
                    HasChilds = pp.Any()
                }
            )
            .Where(x => !x.HasChilds)
            .Select(x => new ParentResponse
            {
                Id = x.Parent.Id!.Value,
                Name = x.Parent.Name,
                Gender = x.Parent.Gender,
                IsParent = true
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PersonResponse>> NoDocumentsAsync()
    {
        var documentsCount = await _context.Documents.CountAsync();
        return await _context.People
            .AsNoTracking()
            .GroupJoin(
                _context.PeopleDocuments,
                p => p.Id,
                pd => pd.PersonId,
                (p, pd) => new
                {
                    Person = p,
                    DocumentsCount = pd.Count()
                }
            )
            .Where(g => g.Person.IsActive && g.DocumentsCount < documentsCount)
            .Select(g => new PersonResponse
            {
                Id = g.Person.Id!.Value,
                Name = g.Person.Name,
                Gender = g.Person.Gender,
                DOB = DateTime.UtcNow,
                Day = g.Person.Day,
                DegreeId = 0,
                IsActive = g.Person.IsActive
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PersonResponse>> NoGodparentsAsync()
    {
        return await _context.People
            .AsNoTracking()
            .GroupJoin(
                _context.ParentsPeople,
                p => p.Id,
                pp => pp.PersonId,
                (p, pp) => new
                {
                    Person = p,
                    HasGodparents = pp.Any(x => !x.IsParent)
                }
            )
            .Where(x => x.Person.IsActive && !x.HasGodparents)
            .Select(x => new PersonResponse
            {
                Id = x.Person.Id!.Value,
                Name = x.Person.Name,
                Gender = x.Person.Gender,
                DOB = DateTime.UtcNow,
                Day = x.Person.Day,
                DegreeId = 0,
                IsActive = x.Person.IsActive
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PersonResponse>> NoPaymentAsync()
    {
        var totalCharges = await _context.Charges.CountAsync();
        return await _context.People
            .AsNoTracking()
            .GroupJoin(
                _context.PeopleCharges,
                p => p.Id,
                pc => pc.PersonId,
                (p, pc) => new
                {
                    Person = p,
                    PeopleChargesCount = pc.Count()
                }
            )
            .Where(x => x.Person.IsActive && x.PeopleChargesCount < totalCharges)
            .Select(x => new PersonResponse
            {
                Id = x.Person.Id!.Value,
                Name = x.Person.Name,
                Gender = x.Person.Gender,
                DOB = DateTime.UtcNow,
                Day = x.Person.Day,
                DegreeId = 0,
                IsActive = x.Person.IsActive
            })
            .ToListAsync();
    }
}