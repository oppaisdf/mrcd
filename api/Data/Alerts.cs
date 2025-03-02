using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IAlertRepository
{
    Task<ushort> NoPaymentCountAsync();
    Task<IEnumerable<PersonResponse>> NoPaymentAsync();
}

public class AlertRepository(
    MerContext context
) : IAlertRepository
{
    private readonly MerContext _context = context;

    public async Task<IEnumerable<PersonResponse>> NoPaymentAsync()
    {
        var totalCharges = await _context.Charges.CountAsync();
        return await _context.People
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
}