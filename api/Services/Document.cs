using api.Common;
using api.Data;
using api.Models.Responses;

namespace api.Services;

public interface IDocumentService
{
    Task AssignAsync(string userId, short documentId, int personId);
    Task UnassignAsync(string userId, short documentId, int personId);
    Task<DocumentResponse> FindBydIdAsync(string userId, short id);
}

public class DocumentService(
    IDocumentRepository repo,
    ILogService logs
) : IDocumentService
{
    private readonly IDocumentRepository _repo = repo;
    private readonly ILogService _logs = logs;

    private async Task AssignUnssingAsync(
        short documentId,
        int personId,
        bool isAssing
    )
    {
        var alreadyExists = await _repo.NotExistsOrAlreadyRegisterAsync(documentId, personId)
            ?? throw new DoesNotExistsException("El documento o el confirmando no existe");
        if (alreadyExists && isAssing) throw new BadRequestException("Ya se ha registrado el documento al confirmando");
        if (isAssing) await _repo.AssignAsync(documentId, personId);
        else await _repo.UnassingAsync(documentId, personId);
    }

    public async Task AssignAsync(
        string userId,
        short documentId,
        int personId
    )
    {
        await _logs.RegisterCreationAsync(userId, $"Documento {documentId} a persona {personId}");
        await AssignUnssingAsync(documentId, personId, true);
    }

    public async Task UnassignAsync(
        string userId,
        short documentId,
        int personId
    )
    {
        await _logs.RegisterUpdateAsync(userId, $"Documento {documentId} a persona {personId}");
        await AssignUnssingAsync(documentId, personId, false);
    }

    public async Task<DocumentResponse> FindBydIdAsync(
        string userId,
        short id
    )
    {
        await _logs.RegisterReadingAsync(userId, $"Documento {id}");
        return await _repo.GetByIdAsync(id) ?? throw new DoesNotExistsException("El documennto que busca no existe");
    }
}