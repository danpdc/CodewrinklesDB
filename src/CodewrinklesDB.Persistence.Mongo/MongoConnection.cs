using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CodewrinklesDB.Persistence.Mongo;

public class MongoConnection
{
    public MongoConnection(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value ?? throw new ArgumentException("MongoDB Options cannot be null.");
        ArgumentException.ThrowIfNullOrEmpty(settings.DatabaseName, nameof(settings.DatabaseName));
        ArgumentException.ThrowIfNullOrEmpty(settings.ConnectionString, nameof(settings.ConnectionString));
        ArgumentException.ThrowIfNullOrEmpty(settings.PendingAcceptanceCollection, 
            nameof(settings.PendingAcceptanceCollection));
        ArgumentException.ThrowIfNullOrEmpty(settings.ActiveNodesCollection, 
            nameof(settings.ActiveNodesCollection));
        Client = new MongoClient(settings.ConnectionString);
        DatabaseName = settings.DatabaseName;
        PendingAcceptanceCollection = settings.PendingAcceptanceCollection;
        ActiveNodesCollection = settings.ActiveNodesCollection;
    }
    public MongoClient Client { get; }
    public string DatabaseName { get; }
    public string PendingAcceptanceCollection { get; }
    public string ActiveNodesCollection { get; }
}