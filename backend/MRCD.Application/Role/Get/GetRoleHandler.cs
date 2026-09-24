using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.Get;

internal sealed class GetRoleHandler(
    IRoleRepository repo
) : IQueryHandler<List<Domain.Role.Role>>
{
    public Task<Result<List<Domain.Role.Role>>> HandleAsync(
        CancellationToken cancellationToken
    ) => repo
        .ToListAsync(cancellationToken)
        .ContinueWith(r => Result<List<Domain.Role.Role>>.Success(r.Result), cancellationToken);
}