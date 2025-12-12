using Microsoft.Extensions.DependencyInjection;

namespace MRCD.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    ) => services;
}