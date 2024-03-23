using CodewrinklesDB.NodeManagement.Discovery;

namespace CodewrinklesDB.NodeManagement.Abstractions;

public interface IDiscoverySender
{
    Task SendDiscoveryMessageAsync(NewNodeMessage newNode, CancellationToken stoppingToken);
}