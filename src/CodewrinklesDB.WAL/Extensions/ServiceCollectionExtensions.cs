using CodewrinklesDB.WAL.Indexer;
using Microsoft.Extensions.DependencyInjection;

namespace CodewrinklesDB.WAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterWriteAheadLog(this IServiceCollection services)
    {
        services.AddSingleton<Indexer.Indexer>();
        services.AddSingleton<MessagingService>();
        services.AddHostedService<IndexerWorker>();
        return services;
    }
}