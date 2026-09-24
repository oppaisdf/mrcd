using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Charge.Contracts;
using MRCD.Domain.Common;
using MRCD.Application.Services;

namespace MRCD.Application.Charge.Add;

internal sealed class AddChargeHandler(
    IChargeRepository repo,
    IPersistenceContext save,
    AuditLog<AddChargeHandler> logs
) : ICommandHandler<AddChargeCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddChargeCommand command,
        CancellationToken cancellationToken
    )
    {
        var charge = Domain.Charge.Charge.Create(command.Name, command.Amount);
        if (!charge.IsSuccess)
            return Result<Guid>.Failure(charge.Error!);
        var normalizedName = StringNormalizer.NormalizeString(charge.Value!.Name);
        var charges = await repo.ToListAsync(cancellationToken);
        var normalizedNames = charges
            .Select(c => StringNormalizer.NormalizeString(c.Name));
        if (normalizedNames.Contains(normalizedName))
            return Result<Guid>.Failure("El nombre del cobro ya está en uso");
        repo.Add(charge.Value!);
        await save.SaveChangesAsync(cancellationToken);
        logs.Write(command.UserId, "Charge {charge} with ID {id} has been created.", command.Name, charge.Value!.ID);
        return Result<Guid>.Success(charge.Value!.ID);
    }
}