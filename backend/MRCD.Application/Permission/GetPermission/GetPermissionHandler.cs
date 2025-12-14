using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.GetPermission;

internal sealed class GetPermissionHandler(
    IPermissionRepository repo
) : IQueryHandler<List<Domain.Role.Permission>>
{
    private readonly IPermissionRepository _repo = repo;

    public Task<Result<List<Domain.Role.Permission>>> HandleAsync(
        CancellationToken cancellationToken
    ) => _repo
        .ToListAsync(cancellationToken)
        .ContinueWith(r => Result<List<Domain.Role.Permission>>.Success(r.Result), cancellationToken);
}