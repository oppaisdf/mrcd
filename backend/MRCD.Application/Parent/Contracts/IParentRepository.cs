using MRCD.Application.Common;
using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Parent.Contracts;

public interface IParentRepository
{
    void Add(Domain.Parent.Parent parent);
    Task<bool> AlreadyExists(string normalizedParentName, CancellationToken cancellationToken);
    Task<List<ParentByPersonDTO>> ByPersonToListAsync(Guid personId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid parentId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid parentId, CancellationToken cancellationToken);
    Task<Domain.Parent.Parent?> GetByIdAsync(Guid parentId, CancellationToken cancellationToken);
    Task<Domain.Parent.Parent?> GetByNameAsync(string normalizedName, CancellationToken cancellationToken);
    Task<List<ParentByPersonDTO>> FilteredByActivePersonToListAsync(CancellationToken cancellationToken);
    Task<Pagination<ParentDTO>> ToListAsync(int page, int size, string? normalizedParentName, CancellationToken cancellationToken);
    Task<Pagination<ParentDTO>> NoChildrenToListAsync(int page, int size, CancellationToken cancellationToken);
}