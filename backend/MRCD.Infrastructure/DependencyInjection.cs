using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Security;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Security;
using MRCD.Infrastructure.Caching;
using MRCD.Infrastructure.Repositories;
using MRCD.Infrastructure.Security;

namespace MRCD.Infrastructure;

public static class DependencyInjection
{
    private static IServiceCollection AddRepositories(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        const string repoNamespace = "MRCD.Infrastructure.Repositories";
        var repos = assembly.GetTypes()
            .Where(r =>
                r is { IsClass: true, IsAbstract: false }
                && r.Name.EndsWith("Repository", StringComparison.Ordinal)
                && r.Namespace is not null
                && r.Namespace.StartsWith(repoNamespace, StringComparison.Ordinal)
            )
            .ToList();
        foreach (var repo in repos)
        {
            var serviceIfaces = repo.GetInterfaces()
                .Where(i =>
                    i.IsInterface
                    && i.Namespace is not null
                    && i.Namespace.StartsWith("MRCD.Application", StringComparison.Ordinal)
                )
                .ToList();
            foreach (var iface in serviceIfaces)
                services.AddScoped(iface, repo);
        }
        services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>));
        return services;
    }

    public static IServiceCollection AddInfrasctructure(
        this IServiceCollection services,
        string dbConnection,
        EncryptionOptions encryptionOptions
    )
    {
        services.AddSingleton(Options.Create(encryptionOptions));
        services.AddSingleton<IEncryptionService, AesGcmEnryptionService>();
        services.AddDbContext<IPersistenceContext, Persistence.AppContext>(options =>
        {
            options.UseMySql(
                dbConnection,
                ServerVersion.AutoDetect(dbConnection)
            );
        });
        services.AddRepositories(typeof(DependencyInjection).Assembly);
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IPermissionCache, DistributedPermissionCache>();
        services.AddScoped<IPermissionReader, EFPermmissionReader>();
        return services;
    }
}