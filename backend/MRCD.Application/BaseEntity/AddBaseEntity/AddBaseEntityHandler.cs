using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Factories;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.AddBaseEntity;

internal sealed class AddBaseEntityHandler<TEntity>(
    IBaseEntityRepository<TEntity> repo,
    IBaseEntityFactory<TEntity> fact,
    IPersistenceContext save,
    ICommonService service,
    ILogger<AddBaseEntityHandler<TEntity>> logs
) : ICommandHandler<AddBaseEntityCommand, Guid>
    where TEntity : Domain.Common.BaseEntity
{
    private readonly IBaseEntityRepository<TEntity> _repo = repo;
    private readonly IBaseEntityFactory<TEntity> _fact = fact;
    private readonly IPersistenceContext _save = save;
    private readonly ICommonService _service = service;
    private readonly ILogger<AddBaseEntityHandler<TEntity>> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddBaseEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var normalizedName = _service.NormalizeString(command.Name.Trim());
        if (!_service.HasOnlyLetters(normalizedName))
            return Result<Guid>.Failure("El nombre solo debe contener letras");
        var records = await _repo.ToListAsync(cancellationToken);
        var normalizedRecords = records
            .Select(r => _service.NormalizeString(r.Name));
        if (normalizedRecords.Contains(normalizedName))
            return Result<Guid>.Failure("El nombre ya se encuentra en uso");
        var newRecord = _fact.Create(command.Name.Trim());
        if (!newRecord.IsSuccess)
            return Result<Guid>.Failure(newRecord.Error!);
        _repo.Add(newRecord.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.LogInformation("Record {record} has been created by user {user}", newRecord.Value!.ID, command.UserId);
        return Result<Guid>.Success(newRecord.Value!.ID);
    }
}