using Microsoft.Extensions.Hosting;

namespace CodewrinklesDB.WAL.Indexer;

public class IndexerWorker : BackgroundService
{
    private readonly Indexer _indexer;

    public IndexerWorker(Indexer indexer)
    {
        _indexer = indexer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _indexer.RebuildIndexState();
        await _indexer.StartListeningAsync();
    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await _indexer.StopListeningAsync();
    }
}