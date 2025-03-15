using api.Models.Entities;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IDocumentRepository
{
    Task<bool?> NotExistsOrAlreadyRegisterAsync(short documentId, int personId);
    Task AssignAsync(short documentId, int personId);
    Task UnassingAsync(short documentId, int personId);
    Task<DocumentResponse?> GetByIdAsync(short id);
}

public class DocumentRepository(
    MerContext context
) : IDocumentRepository
{
    private readonly MerContext _context = context;

    public async Task AssignAsync(
        short documentId,
        int personId
    )
    {
        var personDocument = new PersonDocument
        {
            DocumentId = documentId,
            PersonId = personId
        };
        _context.PeopleDocuments.Add(personDocument);
        await _context.SaveChangesAsync();
    }

    public async Task<DocumentResponse?> GetByIdAsync(
        short id
    )
    {
        return await _context.Documents
            .Where(d => d.Id == id)
            .Select(d => new DocumentResponse(
                d.Name,
                (
                    from p in _context.People
                    where p.IsActive
                    join temp in _context.PeopleDocuments.AsNoTracking().Where(pd => pd.DocumentId == id) on p.Id equals temp.PersonId into tempG
                    from pd in tempG.DefaultIfEmpty()
                    select new BasicPersonResponse
                    {
                        Id = p.Id!.Value,
                        Name = p.Name,
                        Gender = p.Gender,
                        Day = p.Day,
                        IsActive = pd != null
                    }
                ).ToList()
            ))
            .SingleOrDefaultAsync();
    }

    public async Task<bool?> NotExistsOrAlreadyRegisterAsync(
        short documentId,
        int personId
    )
    {
        var result = await (
            from d in _context.Documents.AsNoTracking()
            where d.Id == documentId
            from p in _context.People.AsNoTracking()
                .Where(p => p.Id == personId)
            from pd in _context.PeopleDocuments.AsNoTracking()
                .Where(pd => pd.DocumentId == documentId && pd.PersonId == personId)
                .DefaultIfEmpty()
            select new
            {
                Exists = true,
                AlreadyRegistered = pd != null
            }
        ).SingleOrDefaultAsync();
        if (result == null) return null;
        return result.AlreadyRegistered;
    }

    public async Task UnassingAsync(
        short documentId,
        int personId
    ) => await _context.PeopleDocuments
        .Where(pd => pd.DocumentId == documentId && pd.PersonId == personId)
        .ExecuteDeleteAsync();
}