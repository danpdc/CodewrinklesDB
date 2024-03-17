using CodewrinklesDB.NodeManagement.Discovery;

namespace CodewrinklesDB.TestNodes.Node1;

public class NodeAdvertising(NodeDiscoveryManager manager) : BackgroundService
{
    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        await manager.AdvertiseNewNodeAsync(stoppingToken);

        await base.StartAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await manager.ListenForNodeAdvertisements(stoppingToken);
    }
    
}