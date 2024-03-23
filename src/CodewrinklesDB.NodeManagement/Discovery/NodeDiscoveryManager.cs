using System.Text.Json;
using CodewrinklesDB.Common.Abstractions;
using CodewrinklesDB.Common.Nodes;
using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Discovery.Messages;
using Wolverine;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class NodeDiscoveryManager : IAsyncDisposable
{
    private readonly DiscoverySender _sender;
    private readonly DiscoveryListener _listener;
    private readonly Node _activeNode;
    private readonly IMessageBus _bus;
    private readonly IWriteAheadLogger _wal;
    private readonly INodeRepository _repo;
    public NodeDiscoveryManager(DiscoverySender sender, DiscoveryListener listener, 
        Node activeNode, IMessageBus bus, IWriteAheadLogger wal, INodeRepository repo)
    {
        _sender = sender;
        _listener = listener;
        _activeNode = activeNode;
        _bus = bus;
        _wal = wal;
        _repo = repo;
    }

    public async Task AdvertiseNodeAsync(CancellationToken token)
    {
        await _sender.SendDiscoveryMessageAsync(_activeNode, token);
        var command = new StartListeningForJoiningNodes()
        {
            StoppingToken = token
        };
        await _bus.SendAsync(command);
    }

    public async Task ListenForNodeAdvertisements(CancellationToken stoppingToken)
    {
        _listener.NewNodeDiscovered += ProcessNewNodeAsync;
        await _listener.StartListeningAsync(stoppingToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _listener.StopListeningAsync();
    }

    private async void ProcessNewNodeAsync(object? sender, Node newNode)
    {
        await AddOrUpdateNodeInPendingAcceptanceAsync(newNode);
    }
    
    private async Task AddOrUpdateNodeInPendingAcceptanceAsync(Node newNode)
    {
        var jsonData = JsonSerializer.Serialize(newNode);
        if (await _repo.IsNodePendingAcceptanceAsync(newNode.NodeId))
        {
            await _wal.UpdateAdvertisedNodeAsync(jsonData);
            await _repo.UpdatedNodeInPendingAcceptanceAsync(newNode);
            Console.WriteLine($"Updated node: {newNode.NodeName} in pending acceptance.");
        }
        else
        {
            await _wal.InsertAdvertisedNodeAsync(jsonData);
            await _repo.AddNodeToPendingAcceptanceAsync(newNode);
            Console.WriteLine($"Persisted node: {newNode.NodeName} to pending acceptance.");
        }
    }
}