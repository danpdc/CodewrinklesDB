using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class DiscoveryListener(IDiscoveryListener listener)
{
    public event EventHandler<Node>? NewNodeDiscovered;
    
    public async Task StartListeningAsync(Node listeningNode, CancellationToken cancellationToken)
    {
        listener.NodeDiscovered += OnNodeDiscovered;
        await listener.StartListeningAsync(listeningNode, cancellationToken);
    }

    public async Task StopListeningAsync(Node listeningNode, CancellationToken cancellationToken = default)
    {
        listener.NodeDiscovered -= OnNodeDiscovered;
        await listener.StopListeningAsync(listeningNode, cancellationToken);
    }
    
    private void OnNodeDiscovered(object? sender, Node newNode)
    {
        Console.WriteLine($"New Node discovered: {newNode.NodeName}");
        NewNodeDiscovered?.Invoke(sender, newNode);
    }
}