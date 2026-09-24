using MRCD.Application.Abstracts;

namespace MRCD.Infrastructure.HealthCheck;

internal sealed class DbConnectionChecker(
    Persistence.AppContext context
) : IDbConnectionChecker
{
    public async Task CheckAsync(
        CancellationToken cancellationToken = default
    )
    {
        if (await context.Database.CanConnectAsync(cancellationToken)) return;
        throw new InvalidOperationException("[X] Error al establecer la conexión a la base de datos. :c");
    }
}