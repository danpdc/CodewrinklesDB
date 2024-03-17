using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Infra.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace CodewrinklesDB.NodeManagement.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNodeManagementInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDiscoverySender, BusDiscoverySender>();
        services.AddSingleton<IDiscoveryListener, BusDiscoveryListener>();
        return services;
    }
}