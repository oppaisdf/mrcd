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
        var result = await _context.Documents
            .AsNoTracking()
            .Where(d => d.Id == documentId)
            .GroupJoin(
                _context.PeopleDocuments,
                d => d.Id,
                pd => pd.DocumentId,
                (p, pd) => new
                {
                    AlreadyRegistered = pd.Any(x => x.PersonId == personId)
                }
            )
            .SingleOrDefaultAsync();

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