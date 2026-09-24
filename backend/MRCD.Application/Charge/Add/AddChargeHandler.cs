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
    private readonly IChargeRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly AuditLog<AddChargeHandler> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddChargeCommand command,
        CancellationToken cancellationToken
    )
    {
        var charge = Domain.Charge.Charge.Create(command.Name, command.Amount);
        if (!charge.IsSuccess)
            return Result<Guid>.Failure(charge.Error!);
        var normalizedName = StringNormalizer.NormalizeString(charge.Value!.Name);
        var charges = await _repo.ToListAsync(cancellationToken);
        var normalizedNames = charges
            .Select(c => StringNormalizer.NormalizeString(c.Name));
        if (normalizedNames.Contains(normalizedName))
            return Result<Guid>.Failure("El nombre del cobro ya está en uso");
        _repo.Add(charge.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.Write(command.UserId, "Charge {charge} with ID {id} has been created.", command.Name, charge.Value!.ID);
        return Result<Guid>.Success(charge.Value!.ID);
    }
}