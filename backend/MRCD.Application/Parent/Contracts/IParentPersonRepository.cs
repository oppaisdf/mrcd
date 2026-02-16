using MRCD.Domain.Parent;

namespace MRCD.Application.Parent.Contracts;

public interface IParentPersonRepository
{
    void Add(ParentPerson parentPerson);
    void Del(ParentPerson parentPerson);
    Task<ParentPerson?> GetAsync(Guid personId, Guid parentId, bool isParent, CancellationToken cancellationToken);
    Task<int> AssignedCountAsync(Guid personId, bool isParent, CancellationToken cancellationToken);
}