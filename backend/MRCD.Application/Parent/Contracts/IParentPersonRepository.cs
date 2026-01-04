using MRCD.Domain.Parent;

namespace MRCD.Application.Parent.Contracts;

public interface IParentPersonRepository
{
    void Add(ParentPerson parentPerson);
}