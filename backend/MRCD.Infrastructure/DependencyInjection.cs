using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRCD.Application.Abstracts;

namespace MRCD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrasctructure(
        this IServiceCollection services,
        string dbConnection
    )
    {
        services.AddDbContext<IPersistenceContext, Persistence.AppContext>(options =>
        {
            options.UseMySql(
                dbConnection,
                ServerVersion.AutoDetect(dbConnection)
            );
        });
        return services;
    }
}