using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Discovery;
using Microsoft.Extensions.DependencyInjection;

namespace CodewrinklesDB.NodeManagement.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNodeDiscovery(this IServiceCollection services)
    {
        services.AddSingleton<DiscoverySender>();
        services.AddSingleton<DiscoveryListener>();
        services.AddSingleton<NodeDiscoveryManager>();
        return services;
    }
}