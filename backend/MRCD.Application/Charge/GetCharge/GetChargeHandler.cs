using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Charge.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Charge.GetCharge;

internal sealed class GetCharge(
    IChargeRepository repo
) : IQueryHandler<IEnumerable<Domain.Charge.Charge>>
{
    private readonly IChargeRepository _repo = repo;

    public Task<Result<IEnumerable<Domain.Charge.Charge>>> HandleAsync(
        CancellationToken cancellationToken
    ) => _repo
        .ToListAsync(cancellationToken)
        .ContinueWith(r =>
            Result<IEnumerable<Domain.Charge.Charge>>.Success(r.Result),
            cancellationToken
        );
}