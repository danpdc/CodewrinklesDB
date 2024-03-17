using System.Text.Json;
using Azure.Messaging.ServiceBus;
using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Discovery;

namespace CodewrinklesDB.NodeManagement.Infra.MessageBus;

public class BusDiscoverySender : IDiscoverySender
{
    private const string ConnectionString =
        "Endpoint=sb://codewrinklesdb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rvB866Va0+2nJ1a4QolKxOn7XcVKYZ7Wk+ASbASIKms=";
    
    public async Task SendDiscoveryMessageAsync(NewNodeMessage newNode, CancellationToken stoppingToken)
    {
        var client = new ServiceBusClient(ConnectionString);
        var sender = client.CreateSender("noderegistration");
        var jsonMessage = JsonSerializer.Serialize(newNode);
        var busMessage = new ServiceBusMessage(jsonMessage);

        try
        {
            await sender.SendMessageAsync(busMessage, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending discovery message: {ex.Message}");
        }
        finally
        {
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}