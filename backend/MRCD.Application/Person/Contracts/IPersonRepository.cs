namespace MRCD.Application.Person.Contracts;

public interface IPersonRepository
{
    Task<bool> ExistsActiveAsync(Guid personId, CancellationToken cancellationToken);
}