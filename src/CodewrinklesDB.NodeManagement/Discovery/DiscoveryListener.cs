using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class DiscoveryListener
{
    private readonly IDiscoveryListener _listener;
    private readonly Node _listeningNode;
    public DiscoveryListener(IDiscoveryListener listener, Node listeningNode)
    {
        _listener = listener;
        _listeningNode = listeningNode;
    }
    public event EventHandler<Node>? NewNodeDiscovered;
    
    public async Task StartListeningAsync(CancellationToken cancellationToken = default)
    {
        _listener.NodeDiscovered += OnNodeDiscovered;
        await _listener.StartListeningAsync(_listeningNode, cancellationToken);
    }

    public async Task StopListeningAsync(CancellationToken cancellationToken = default)
    {
        _listener.NodeDiscovered -= OnNodeDiscovered;
        await _listener.StopListeningAsync(_listeningNode, cancellationToken);
    }
    
    private void OnNodeDiscovered(object? sender, Node newNode)
    {
        NewNodeDiscovered?.Invoke(sender, newNode);
    }
    
    
}