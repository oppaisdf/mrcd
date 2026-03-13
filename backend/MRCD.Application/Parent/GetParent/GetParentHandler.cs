using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.GetParent;

internal sealed class GetParentHandler(
    IParentRepository repo,
    ICommonService service
) : IQueryHandler<Pagination<ParentDTO>, GetParentQuery>
{
    private readonly IParentRepository _repo = repo;
    private readonly ICommonService _service = service;

    public async Task<Result<Pagination<ParentDTO>>> HandleAsync(
        GetParentQuery query,
        CancellationToken cancellationToken
    )
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size is < 1 or > 30 ? 30 : query.Size;

        var parents = query.Alert is null || query.Alert != Alert.Common.AlertType.ParentsLonely
            ? await _repo
                .ToListAsync(
                    page,
                    size,
                    string.IsNullOrWhiteSpace(query.ParentName)
                        ? null
                        : _service.NormalizeString(query.ParentName),
                    cancellationToken
                )
            : await _repo.NoChildrenToListAsync(
                page,
                size,
                cancellationToken
            );
        return Result<Pagination<ParentDTO>>.Success(parents);
    }
}