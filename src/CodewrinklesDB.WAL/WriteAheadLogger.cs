using CodewrinklesDB.Common.Abstractions;
using CodewrinklesDB.WAL.Files;

namespace CodewrinklesDB.WAL;

public class WriteAheadLogger : IWriteAheadLogger
{
    private readonly Indexer.Indexer _indexer;

    public WriteAheadLogger(Indexer.Indexer indexer)
    {
        _indexer = indexer;
    }


    public async Task InsertAdvertisedNodeAsync(string nodeData)
    {
        var logFilePath = FileManager.GetCurrentLogFilePath();
        var index = await _indexer.GetNextIndexAsync();
        var log = new Log(index, LogType.InsertAdvertisedNode, nodeData, DateTimeOffset.UtcNow);
        await FileLogger.InsertLogAsync(log, logFilePath);
    }

    public async Task UpdateAdvertisedNodeAsync(string nodeData)
    {
        var logFilePath = FileManager.GetCurrentLogFilePath();
        var index = await _indexer.GetNextIndexAsync();
        var log = new Log(index, LogType.UpdateAdvertisedNode, nodeData, DateTimeOffset.UtcNow);
        await FileLogger.InsertLogAsync(log, logFilePath);
    }
}