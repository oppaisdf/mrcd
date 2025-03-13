using api.Common;
using api.Data;

namespace api.Services;

public interface IDocumentService
{
    Task AssignAsync(string userId, short documentId, int personId);
    Task UnassignAsync(string userId, short documentId, int personId);
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
            ?? throw new DoesNotExistsException("El documento no existe");
        if (alreadyExists) throw new BadRequestException("Ya se ha registrado el documento al confirmando");
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
}