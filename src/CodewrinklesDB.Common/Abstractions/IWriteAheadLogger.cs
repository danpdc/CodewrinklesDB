using CodewrinklesDB.Common.Nodes;

namespace CodewrinklesDB.Common.Abstractions;

public interface IWriteAheadLogger
{
    Task LogAdvertisedNodeAsync(Node advertisedNode);
}