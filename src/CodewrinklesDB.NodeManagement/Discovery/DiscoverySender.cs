using CodewrinklesDB.Common.Nodes;
using CodewrinklesDB.NodeManagement.Abstractions;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class DiscoverySender(IDiscoverySender sender)
{
    public async Task SendDiscoveryMessageAsync(Node newNode, CancellationToken stoppingToken)
    {
        try
        {
            // Construct the discovery message
            var message = new NewNodeMessage
            {
                MessageId = Guid.NewGuid(),
                Node = newNode,
                Timestamp = DateTimeOffset.UtcNow
            };

            await sender.SendDiscoveryMessageAsync(message, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending discovery message: {ex.Message}");
        }
    }
}