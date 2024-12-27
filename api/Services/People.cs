using System.Text;
using api.Common;
using api.Context;
using api.Models.Entities;
using api.Models.Filters;
using api.Models.Requests;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IPeopleService
{
    Task<int> CreateAsync(string userId, PeopleRequest request);
    Task<(ICollection<PersonResponse> people, string pages)> GetAsync(string userId, PeopleFilter filter);
    Task<PersonResponse> GetByIdAsync(string userId, int id);
    Task UpdateAsync(string userId, int id, PeopleRequest request);
}

public partial class PeopleService(
    MerContext context,
    ILogService logs,
    ICommonFunctions functions
) : IPeopleService
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;
    private readonly ICommonFunctions _functions = functions;

    #region "Métodos privados"
    private async Task CreateParentsAsync(
        int personId,
        ICollection<ParentRequest> request
    )
    {
        _context.Parents.AddRange(request
            .Select(p => new Parent
            {
                Name = p.Name.Trim(),
                Gender = p.Gender,
                PersonId = personId
            })
            .ToList()
        );
        await _context.SaveChangesAsync();
    }

    private async Task UpdateParentsAsync(
        int personId,
        ICollection<ParentRequest> request
    )
    {
        _context.Parents.RemoveRange(
            await _context.Parents.Where(p => p.PersonId == personId).ToListAsync()
        );
        await CreateParentsAsync(personId, request);
    }

    private static void ValidateBirth(
        DateTime birth
    )
    {
        var diff = DateTime.UtcNow.Year - birth.Year;
        if (diff < 0) throw new BadRequestException("Ni ha nacido");
        if (diff < 14) throw new BadRequestException("Muy jóven para el sacramento");
        if (diff > 30) throw new BadRequestException("Muy grande para este grupo");
    }

    public string GetHash(
        string name
    )
    {
        byte[] bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(_functions.GetNormalizedText(name)));
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }
    #endregion

    public async Task<int> CreateAsync(
        string userId,
        PeopleRequest request
    )
    {
        ValidateBirth(request.DOB!.Value);
        var hash = GetHash(request.Name!);

        var degreeExists = await _context.Degrees.AnyAsync(d => d.Id == request.DegreeId);
        if (!degreeExists) throw new DoesNotExistsException("El grado académico no existe");
        var alreadyExists = await _context.People.AnyAsync(p => p.Hash == hash);
        if (alreadyExists) throw new AlreadyExistsException("El confirmando ya existe");

        var person = new Person
        {
            Name = request.Name!,
            Gender = request.Gender!.Value,
            DOB = request.DOB!.Value,
            Day = request.Day!.Value,
            Parish = request.Parish,
            DegreeId = request.DegreeId!.Value,
            Address = request.Address!,
            Hash = hash,
            IsActive = true,
            Phone = request.Phone
        };

        _context.People.Add(person);

        var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SaveChangesAsync();
            if (request.Parents != null && request.Parents.Count > 0)
                await CreateParentsAsync(person.Id!.Value, request.Parents);
            await _logs.RegisterCreationAsync(userId, $"Creó confirmando {person.Id}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }

        return person.Id!.Value;
    }

    public async Task<(ICollection<PersonResponse> people, string pages)> GetAsync(
        string userId,
        PeopleFilter filter
    )
    {
        await _logs.RegisterReadingAsync("Todos los confirmandos");
        var query = _context.People.AsQueryable();

        if (filter.Gender != null) query = query.Where(p => p.Gender == filter.Gender);
        if (filter.Day != null) query = query.Where(p => p.Day == filter.Day);
        if (filter.DegreeId != null) query = query.Where(p => p.DegreeId == filter.DegreeId);
        if (filter.IsActive != null) query = query.Where(p => p.IsActive == filter.IsActive);

        int counter;
        ICollection<PersonResponse> page;
        if (string.IsNullOrWhiteSpace(filter.Name))
        {
            counter = (int)Math.Ceiling(await query.CountAsync() / 15.0);
            page = await query
                .Skip((filter.Page - 1) * 15)
                .Take(15)
                .Select(p => new PersonResponse
                {
                    Id = p.Id!.Value,
                    Name = p.Name,
                    Gender = p.Gender,
                    DOB = p.DOB,
                    Day = p.Day,
                    DegreeId = p.DegreeId,
                    Address = "",
                    IsActive = p.IsActive
                }).ToListAsync();
        }
        else
        {
            var people = await query.ToListAsync();
            people.ForEach(p => p.Hash = _functions.GetNormalizedText(p.Name));
            people = people.Where(p => p.Hash.Contains(_functions.GetNormalizedText(filter.Name))).ToList();
            counter = (int)Math.Ceiling(people.Count / 15.0);
            page = people
                .Skip((filter.Page - 1) * 15)
                .Take(15)
                .Select(p => new PersonResponse
                {
                    Id = p.Id!.Value,
                    Name = p.Name,
                    Gender = p.Gender,
                    DOB = p.DOB,
                    Day = p.Day,
                    DegreeId = p.DegreeId,
                    Address = "",
                    IsActive = p.IsActive
                })
                .ToList();
        }

        return (page, $"{filter.Page}/{counter}");
    }

    public async Task<PersonResponse> GetByIdAsync(
        string userId,
        int id
    )
    {
        await _logs.RegisterReadingAsync(userId, $"Confirmando {id}");
        return await _context.People
            .Where(p => p.Id == id)
            .Select(p => new PersonResponse
            {
                Id = p.Id!.Value,
                Name = p.Name,
                Gender = p.Gender,
                DOB = p.DOB,
                Day = p.Day,
                IsActive = p.IsActive,
                Parish = p.Parish,
                DegreeId = p.DegreeId,
                Address = p.Address,
                Parents = _context.Parents
                    .Where(x => x.PersonId == id)
                    .Select(x => new ParentResponse
                    {
                        Id = x.Id!.Value,
                        Name = x.Name,
                        Gender = x.Gender
                    })
                    .ToList(),
                Godparents = _context.Godparents
                    .Where(x => x.PersonId == id)
                    .Select(x => new ParentResponse
                    {
                        Id = x.Id!.Value,
                        Name = x.Name,
                        Gender = x.Gender
                    })
                    .ToList()
            }).FirstOrDefaultAsync()
            ?? throw new DoesNotExistsException("El confirmando no existe");
    }

    public async Task UpdateAsync(
        string userId,
        int id,
        PeopleRequest request
    )
    {
        var person = await _context.People.FindAsync(id) ?? throw new DoesNotExistsException("El confirmando no existe");
        if (!person.IsActive && request.IsActive != true) throw new BadRequestException("El confirmando debe estar activo para actualizarlo");
        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != person.Name) person.Name = request.Name;
        if (request.Gender != null && request.Gender != person.Gender) person.Gender = request.Gender!.Value;
        if (request.DOB != null && request.DOB != person.DOB)
        {
            ValidateBirth(request.DOB!.Value);
            person.DOB = request.DOB!.Value;
        }
        if (request.Day != null && request.Day != person.Day) person.Day = request.Day!.Value;
        if (request.Parish != null && request.Parish != person.Parish) person.Parish = request.Parish;
        if (request.DegreeId != null && request.DegreeId != person.DegreeId)
        {
            var gradeExists = await _context.Degrees.AnyAsync(d => d.Id == request.DegreeId);
            if (gradeExists) person.DegreeId = request.DegreeId!.Value;
        }
        if (request.Address != null && person.Address != request.Address) person.Address = request.Address;
        if (request.Phone != null && person.Phone != request.Phone) person.Phone = request.Phone;

        var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SaveChangesAsync();
            if (request.Parents != null)
                await UpdateParentsAsync(id, request.Parents);
            await _logs.RegisterUpdateAsync(userId, $"Confirmando {id}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}