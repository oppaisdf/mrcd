using api.Common;
using api.Data;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;

namespace api.Services;

public interface IStageService
{
    Task<IEnumerable<StageResponse>> ToListAsync();
    Task DeleteAsync(string userId, ushort id);
    Task CreateAsync(string userId, StageRequest request);
}

public class StageService(
    IStageRepository repo,
    ICommonService service
) : IStageService
{
    private readonly IStageRepository _repo = repo;
    private readonly ICommonService _service = service;

    public async Task<IEnumerable<StageResponse>> ToListAsync()
    => await _repo
        .ToListAsync()
        .ConfigureAwait(false);

    public async Task DeleteAsync(
        string userId,
        ushort id
    )
    {
        var usingStage = await _repo
            .UsingAsync(id)
            .ConfigureAwait(false);
        if (usingStage) throw new BadRequestException("No se puede eliminar porque la fase de actividad está en uso");
        await _repo
            .DeleteAsync(userId, id)
            .ConfigureAwait(false);
    }

    public async Task CreateAsync(
        string userId,
        StageRequest request
    )
    {
        var stages = await _repo
            .NamesToListAsync()
            .ConfigureAwait(false);
        stages.ForEach(s => s = _service.GetNormalizedText(s));
        if (stages.Contains(request.Name)) throw new AlreadyExistsException("La etapa ya está registrada");
        var stage = new ActivityStage
        {
            Name = request.Name
        };
        await _repo
            .CreateAsync(userId, stage)
            .ConfigureAwait(false);
    }
}