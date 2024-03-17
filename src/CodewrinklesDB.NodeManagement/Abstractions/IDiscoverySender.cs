using CodewrinklesDB.NodeManagement.Discovery;
using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Abstractions;

public interface IDiscoverySender
{
    Task SendDiscoveryMessageAsync(NewNodeMessage newNode, CancellationToken stoppingToken);
}