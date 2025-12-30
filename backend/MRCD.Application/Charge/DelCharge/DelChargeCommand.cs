using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Charge.DelCharge;

public sealed record DelChargeCommand(
    Guid UserId,
    Guid ChargeId
) : ICommand<Result>;