using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class NodeDiscoveryManager(DiscoverySender sender, DiscoveryListener listener, 
    Node activeNode) : IAsyncDisposable
{
    public async Task AdvertiseNewNodeAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (activeNode.ClusterRole == ClusterRole.Unregistered)
                await sender.SendDiscoveryMessageAsync(activeNode, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending discovery message: {ex.Message}");
        }
    }

    public async Task ListenForNodeAdvertisements(CancellationToken stoppingToken)
    {
        await listener.StartListeningAsync(activeNode, stoppingToken);
    }

    public async ValueTask DisposeAsync()
    {
        await listener.StopListeningAsync(activeNode);
    }
}