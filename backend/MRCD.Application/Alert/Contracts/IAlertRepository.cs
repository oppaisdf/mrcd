namespace MRCD.Application.Alert.Contracts;

public interface IAlertRepository
{
    Task<int> ParentsLonelyCountAsync(CancellationToken cancellationToken);
    Task<int> PendingChargesCountAsync(CancellationToken cancellationToken);
    Task<int> PendingDocumentsCountAsync(CancellationToken cancellationToken);
    Task<int> WithoutGodparentsCountAsync(CancellationToken cancellationToken);
}