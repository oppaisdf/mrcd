using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application;

public static class DependencyInjection
{
    private static IServiceCollection RegisterHandlers(
        IServiceCollection services,
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
                if (iface.IsGenericType) continue;
                var def = iface.GetGenericTypeDefinition();
                if (handlers.Contains(def))
                    services.AddScoped(iface, type);
            }
        }
        return services;
    }

    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        RegisterHandlers(services, typeof(DependencyInjection).Assembly);
        return services;
    }
}