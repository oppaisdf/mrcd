using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.GetRole;

internal sealed class GetRoleHandler(
    IRoleRepository repo
) : IQueryHandler<List<Domain.Role.Role>>
{
    private readonly IRoleRepository _repo = repo;

    public Task<Result<List<Domain.Role.Role>>> HandleAsync(
        CancellationToken cancellationToken
    ) => _repo
        .ToListAsync(cancellationToken)
        .ContinueWith(r => Result<List<Domain.Role.Role>>.Success(r.Result), cancellationToken);
}