using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IDocumentRepository
{
    Task<bool?> NotExistsOrAlreadyRegisterAsync(short documentId, int personId);
    Task AssignAsync(short documentId, int personId);
    Task UnassingAsync(short documentId, int personId);
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