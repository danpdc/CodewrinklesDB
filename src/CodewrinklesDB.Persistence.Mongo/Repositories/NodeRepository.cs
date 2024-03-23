using CodewrinklesDB.Common.Nodes;
using CodewrinklesDB.NodeManagement.Abstractions;

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
        var filter = Builders<Node>.Filter.Eq(n => n.NodeId, nodeId);
        var collection = _connection.Client.GetDatabase(_connection.DatabaseName)
            .GetCollection<Node>(_connection.PendingAcceptanceCollection);
        return await collection.Find(filter).AnyAsync();
    }

    public async Task<Node> AddNodeToPendingAcceptanceAsync(Node newNode)
    {
        var collection = _connection.Client.GetDatabase(_connection.DatabaseName)
            .GetCollection<Node>(_connection.PendingAcceptanceCollection);
        await collection.InsertOneAsync(newNode);
        return newNode;
    }

    public async Task<Node> UpdatedNodeInPendingAcceptanceAsync(Node updatedNode)
    {
        var filter = Builders<Node>.Filter.Eq(n => n.NodeId, updatedNode.NodeId);
        var update = Builders<Node>.Update
            .Set(n => n.Capacity, updatedNode.Capacity)
            .Set(n => n.Metadata, updatedNode.Metadata)
            .Set(n => n.Port, updatedNode.Port)
            .Set(n => n.ClusterRole, updatedNode.ClusterRole)
            .Set(n => n.HealthStatus, updatedNode.HealthStatus)
            .Set(n => n.IpAddress, updatedNode.IpAddress)
            .Set(n => n.NodeDescription, updatedNode.NodeDescription)
            .Set(n => n.NodeName, updatedNode.NodeName);
        var collection = _connection.Client.GetDatabase(_connection.DatabaseName)
            .GetCollection<Node>(_connection.PendingAcceptanceCollection);
        await collection.UpdateOneAsync(filter, update);
        return updatedNode;
    }
}