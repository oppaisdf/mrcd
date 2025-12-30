using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Charge.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;

namespace MRCD.Application.Charge.AddCharge;

internal sealed class AddChargeHandler(
    IChargeRepository repo,
    IPersistenceContext save,
    ILogger<AddChargeHandler> logs,
    ICommonService service
) : ICommandHandler<AddChargeCommand, Guid>
{
    private readonly IChargeRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddChargeHandler> _logs = logs;
    private readonly ICommonService _service = service;

    public async Task<Result<Guid>> HandleAsync(
        AddChargeCommand command,
        CancellationToken cancellationToken
    )
    {
        var charge = Domain.Charge.Charge.Create(command.Name, command.Amount);
        if (!charge.IsSuccess)
            return Result<Guid>.Failure(charge.Error!);
        var normalizedName = _service.NormalizeString(charge.Value!.Name);
        var charges = await _repo.ToListAsync(cancellationToken);
        var normalizedNames = charges
            .Select(c => _service.NormalizeString(c.Name));
        if (normalizedNames.Contains(normalizedName))
            return Result<Guid>.Failure("El nombre del cobro ya está en uso");
        _repo.Add(charge.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.LogInformation("Charge {charge} has been created by user {user}", charge.Value!.ID, command.UserId);
        return Result<Guid>.Success(charge.Value!.ID);
    }
}