using Microsoft.EntityFrameworkCore;
using MRCD.Application.Alert.Contracts;

namespace MRCD.Infrastructure.Repositories;

internal sealed class AlertRepository(
    Persistence.AppContext app
) : IAlertRepository
{
    private readonly Persistence.AppContext _app = app;

    public Task<int> ParentsLonelyCountAsync(
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .GroupJoin(
            _app.ParentsPersons,
            p => p.ID,
            pp => pp.ParentId,
            (p, pp) => new
            {
                HasChildren = pp.Any()
            }
        )
        .Where(p => !p.HasChildren)
        .CountAsync(cancellationToken);

    public async Task<int> PendingChargesCountAsync(
        CancellationToken cancellationToken
    )
    {
        var totalCharges = await _app.Charges.CountAsync(cancellationToken);
        return await _app.People
            .GroupJoin(
                _app.PersonCharges,
                p => p.ID,
                pc => pc.PersonId,
                (p, pc) => new
                {
                    Person = p,
                    PersonChargeCount = pc.Count()
                }
            )
            .Where(c => c.Person.IsActive && c.PersonChargeCount < totalCharges)
            .CountAsync(cancellationToken);
    }

    public async Task<int> PendingDocumentsCountAsync(
        CancellationToken cancellationToken
    )
    {
        var documentsCount = await _app.Documents.CountAsync(cancellationToken);
        return await _app.People
            .GroupJoin(
                _app.PersonDocuments,
                p => p.ID,
                pd => pd.PersonId,
                (p, pd) => new
                {
                    Person = p,
                    PersonDocumentCount = pd.Count()
                }
            )
            .Where(d => d.Person.IsActive && d.PersonDocumentCount < documentsCount)
            .CountAsync(cancellationToken);
    }

    public Task<int> WithoutGodparentsCountAsync(
        CancellationToken cancellationToken
    ) => _app.People
        .GroupJoin(
            _app.ParentsPersons,
            p => p.ID,
            pp => pp.PersonId,
            (p, pp) => new
            {
                Person = p,
                hasParents = pp.Any(x => !x.IsParent)
            }
        )
        .Where(p => p.Person.IsActive && !p.hasParents)
        .CountAsync(cancellationToken);
}