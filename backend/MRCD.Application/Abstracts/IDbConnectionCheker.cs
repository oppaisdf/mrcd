namespace MRCD.Application.Abstracts;

public interface IDbConnectionChecker
{
    Task CheckAsync(CancellationToken cancellationToken = default);
}