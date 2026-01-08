namespace MRCD.Application.Person.Contracts;

public interface IPersonRepository
{
    void Add(Domain.Person.Person person);
    Task<bool> AlreadyExistsNameAsync(string normalizedName, CancellationToken cancellationToken);
    Task<bool> AlreadyExistExceptIdAsync(string normalizedName, Guid personId, CancellationToken cancellationToken);
    Task<bool> ExistsActiveAsync(Guid personId, CancellationToken cancellationToken);
    Task<Domain.Person.Person?> GetByIdAsync(Guid personId, CancellationToken cancellationToken);
    Task<List<Domain.Person.Person>> OnlyActiveToListAsync(CancellationToken cancellationToken);
}