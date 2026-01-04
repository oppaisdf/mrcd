namespace MRCD.Application.Person.Contracts;

public interface IPersonRepository
{
    Task<bool> ExistsActiveAsync(Guid personId, CancellationToken cancellationToken);
    Task<List<Domain.Person.Person>> OnlyActiveToListAsync(CancellationToken cancellationToken);
}