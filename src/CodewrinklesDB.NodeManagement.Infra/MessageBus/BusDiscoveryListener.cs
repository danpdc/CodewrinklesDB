using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using CodewrinklesDB.NodeManagement.Abstractions;
using CodewrinklesDB.NodeManagement.Discovery;
using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Infra.MessageBus;

public class BusDiscoveryListener : IDiscoveryListener
{
    private const string ConnectionString =
        "Endpoint=sb://codewrinklesdb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rvB866Va0+2nJ1a4QolKxOn7XcVKYZ7Wk+ASbASIKms=";
    private const string TopicName = "noderegistration";
    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;
    
    public event EventHandler<Node>? NodeDiscovered;
    
    public async Task StartListeningAsync(Node listener, CancellationToken stoppingToken)
    {
        _client = new ServiceBusClient(ConnectionString);
        var subscription = await GetCreateSubscriptionAsync(listener, stoppingToken);
        _processor = _client.CreateProcessor(TopicName, subscription, new ServiceBusProcessorOptions());
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync(stoppingToken);
    }
    
    public async Task StopListeningAsync(Node listener, CancellationToken stoppingToken)
    {
        if (_processor is not null)
        {
            _processor.ProcessMessageAsync -= MessageHandler;
            _processor.ProcessErrorAsync -= ErrorHandler;
            await _processor.StopProcessingAsync(stoppingToken);
            await _processor.DisposeAsync();
        }

        if (_client is not null)
            await _client.DisposeAsync();
    }
    
    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string jsonBody = args.Message.Body.ToString();
        var newNode = JsonSerializer.Deserialize<NewNodeMessage>(jsonBody);
        Console.WriteLine($"Processing advertisement for node: {newNode?.Node?.NodeName}");

        // complete the message. messages is deleted from the subscription. 
        await args.CompleteMessageAsync(args.Message);
        if (newNode?.Node is not null)
            NodeDiscovered?.Invoke(this, newNode.Node);
    }

    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
    
    private static async Task<string> GetCreateSubscriptionAsync(Node listeningNode, 
        CancellationToken stoppingToken)
    {
        var adminClient = new ServiceBusAdministrationClient(ConnectionString);
        var subscriptionExists = await adminClient.SubscriptionExistsAsync(TopicName,
            listeningNode.NodeName, stoppingToken);

        if (subscriptionExists) return listeningNode.NodeName;
        
        await adminClient.CreateSubscriptionAsync(TopicName, 
            listeningNode.NodeName, stoppingToken);
        return listeningNode.NodeName;

    }
}
