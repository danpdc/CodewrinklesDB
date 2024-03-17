using CodewrinklesDB.NodeManagement.Discovery.Messages;
using CodewrinklesDB.NodeManagement.Nodes;
using Wolverine;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class NodeDiscoveryManager : IAsyncDisposable
{
    private readonly DiscoverySender _sender;
    private readonly DiscoveryListener _listener;
    private Node _activeNode;
    private readonly IMessageBus _bus;
    public NodeDiscoveryManager(DiscoverySender sender, DiscoveryListener listener, 
        Node activeNode, IMessageBus bus)
    {
        _sender = sender;
        _listener = listener;
        _activeNode = activeNode;
        _bus = bus;
    }
    public async Task AdvertiseNewNodeAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (_activeNode.ClusterRole == ClusterRole.Unregistered)
                await _sender.SendDiscoveryMessageAsync(_activeNode, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending discovery message: {ex.Message}");
        }
    }

    public async Task AdvertiseNodeAsync2(CancellationToken token)
    {
        var command = new StartListeningForJoiningNodes()
        {
            StoppingToken = token
        };
        await _bus.SendAsync(command);
    }

    public async Task ListenForNodeAdvertisements(CancellationToken stoppingToken)
    {
        await _listener.StartListeningAsync(_activeNode, stoppingToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _listener.StopListeningAsync(_activeNode);
    }
}