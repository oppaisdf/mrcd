using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Charge.AddCharge;

public sealed record AddChargeCommand(
    Guid UserId,
    string Name,
    decimal Amount
) : ICommand<Guid>;