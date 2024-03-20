using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Abstractions;

public interface IDiscoveryListener
{
    Task StartListeningAsync(Node listener, CancellationToken stoppingToken);
    event EventHandler<Node> NodeDiscovered;
    Task StopListeningAsync(Node listener, CancellationToken stoppingToken = default);
    
}