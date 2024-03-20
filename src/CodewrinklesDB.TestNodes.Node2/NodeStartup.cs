using CodewrinklesDB.NodeManagement.Discovery;

namespace CodewrinklesDB.TestNodes.Node2;

public class NodeStartup(NodeDiscoveryManager manager) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await manager.AdvertiseNodeAsync(stoppingToken);
    }
}