using MRCD.Application.Abstracts;

namespace MRCD.API.Extensions;

public static class HostExtensions
{
    public static async Task CheckDatabaseConnectionAsync(
        this IHost host
    )
    {
        await using var scope = host.Services.CreateAsyncScope();
        var checker = scope.ServiceProvider.GetRequiredService<IDbConnectionChecker>();
        await checker.CheckAsync();
    }
}