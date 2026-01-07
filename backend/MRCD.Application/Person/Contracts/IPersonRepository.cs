namespace MRCD.Application.Person.Contracts;

public interface IPersonRepository
{
    void Add(Domain.Person.Person person);
    Task<bool> AlreadyExistsNameAsync(string normalizedName, CancellationToken cancellationToken);
    Task<bool> ExistsActiveAsync(Guid personId, CancellationToken cancellationToken);
    Task<List<Domain.Person.Person>> OnlyActiveToListAsync(CancellationToken cancellationToken);
}