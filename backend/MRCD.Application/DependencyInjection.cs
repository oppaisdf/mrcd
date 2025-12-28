using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MRCD.Application.Abstracts.Factories;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Services.CommonService;

namespace MRCD.Application;

public static class DependencyInjection
{
    private static IServiceCollection RegisterHandlers(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var handlers = new[]
        {
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
        return services;
    }
}