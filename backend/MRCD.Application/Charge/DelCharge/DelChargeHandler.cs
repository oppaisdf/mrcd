using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Charge.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Charge.DelCharge;

internal sealed class DelChargeHandler(
    IChargeRepository repo,
    ILogger<DelChargeHandler> logs
) : ICommandHandler<DelChargeCommand>
{
    private readonly IChargeRepository _repo = repo;
    private readonly ILogger<DelChargeHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelChargeCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.ExistsIdAsync(command.ChargeId, cancellationToken);
        if (!exists)
            return Result.Failure("El cobro no existe");
        await _repo.DeleteAsync(command.ChargeId, cancellationToken);
        _logs.LogInformation("Charge {charge} has been deleted by user {user}", command.ChargeId, command.UserId);
        return Result.Success();
    }
}