using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MRCD.Application.Abstracts.Factories;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.AddBaseEntity;
using MRCD.Application.BaseEntity.GetBaseEntity;
using MRCD.Application.Security;
using MRCD.Application.Services.CommonService;

namespace MRCD.Application;

public static class DependencyInjection
{
    private static IServiceCollection RegisterHandlers(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        services.AddScoped(typeof(IBaseQueryHandler<>), typeof(GetBaseEntityHandler<>));
        var handlers = new[]
        {
            typeof(IQueryHandler<>),
            typeof(IQueryHandler<,>),
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>)
        };

        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsClass || type.IsAbstract) continue;
            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType) continue;
                var def = iface.GetGenericTypeDefinition();
                if (handlers.Contains(def))
                    services.AddScoped(iface, type);
            }
        }

        var entityTypes = assembly.GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && typeof(Domain.Common.BaseEntity).IsAssignableFrom(t));
        foreach (var entityType in entityTypes)
        {
            var serviceType = typeof(ICommandHandler<,,>)
                .MakeGenericType(typeof(AddBaseEntityCommand), typeof(Guid), entityType);

            var implType = typeof(AddBaseEntityHandler<>)
                .MakeGenericType(entityType);

            services.AddScoped(serviceType, implType);
        }
        return services;
    }

    private static IServiceCollection RegisterFactories(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var factories = new[]
        {
            typeof(IBaseEntityFactory<>)
        };
        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsClass || type.IsAbstract) continue;
            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType) continue;
                var def = iface.GetGenericTypeDefinition();
                if (!factories.Contains(def)) continue;
                services.AddSingleton(iface, type);
            }
        }
        return services;
    }

    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        services.RegisterFactories(typeof(DependencyInjection).Assembly);
        services.RegisterHandlers(typeof(DependencyInjection).Assembly);
        // Services
        services.AddSingleton<ICommonService, CommonService>();
        services.AddScoped<PermissionService>();
        return services;
    }
}