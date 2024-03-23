using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Abstractions;

public interface IWriteAheadLogger
{
    Task LogAdvertisedNodeAsync(Node advertisedNode);
}