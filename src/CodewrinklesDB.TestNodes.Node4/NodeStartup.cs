using CodewrinklesDB.NodeManagement.Discovery;

namespace CodewrinklesDB.TestNodes.Node4;

public class NodeStartup(NodeDiscoveryManager manager) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await manager.AdvertiseNodeAsync2(stoppingToken);
    }
}