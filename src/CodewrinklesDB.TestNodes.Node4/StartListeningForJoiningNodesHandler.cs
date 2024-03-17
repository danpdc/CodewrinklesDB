using CodewrinklesDB.NodeManagement.Discovery;
using CodewrinklesDB.NodeManagement.Discovery.Messages;

namespace CodewrinklesDB.TestNodes.Node4;

public class StartListeningForJoiningNodesHandler
{
    public static async Task HandleAsync(StartListeningForJoiningNodes message, 
        NodeDiscoveryManager manager)
    {
        await manager.ListenForNodeAdvertisements(message.StoppingToken);
    }
}