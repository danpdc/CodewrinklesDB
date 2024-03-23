using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.Persistence.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodewrinklesDB.Persistence.Mongo.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.Settings));
        services.AddSingleton<MongoConnection>();
        services.AddSingleton<INodeRepository, NodeRepository>();
        return services;
    }
}