using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.DelParent;

internal sealed class DelParentHandler(
    IParentRepository repo,
    ILogger<DelParentHandler> logs
) : ICommandHandler<DelParentCommand>
{
    private readonly IParentRepository _repo = repo;
    private readonly ILogger<DelParentHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelParentCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.ExistsAsync(command.ParentId, cancellationToken);
        if (!exists)
            return Result.Failure("El padre/padrino no existe");
        await _repo.DeleteAsync(command.ParentId, cancellationToken);
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("Parent {parent} has been deleted.", command.ParentId);
        }
        return Result.Success();
    }
}