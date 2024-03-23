using CodewrinklesDB.Common.Nodes;

namespace CodewrinklesDB.Common.Abstractions;

public interface IWriteAheadLogger
{
    Task InsertAdvertisedNodeAsync(string nodeData);
    Task UpdateAdvertisedNodeAsync(string nodeData);
}