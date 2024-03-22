using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Nodes;
using MongoDB.Driver;

namespace CodewrinklesDB.Persistence.Mongo.Repositories;

public class NodeRepository : INodeRepository
{
    private readonly MongoConnection _connection;

    public NodeRepository(MongoConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<bool> IsNodePendingAcceptanceAsync(Guid nodeId)
    {
        var filter = Builders<Node>.Filter.Exists(n => n.NodeId == nodeId);
        var collection = _connection.Client.GetDatabase(_connection.DatabaseName)
            .GetCollection<Node>(_connection.PendingAcceptanceCollection);
        return await collection.Find(filter).AnyAsync();
    }
}