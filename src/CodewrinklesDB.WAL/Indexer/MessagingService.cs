using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.WAL.Indexer;

public class MessagingService
{
    private const string ConnectionString =
        "{Service Bus Connection string}";
    private const string TopicName = "indexer";
    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;
    private readonly Node _currentNode;

    public MessagingService(Node currentNode)
    {
        _currentNode = currentNode;
    }
    
    public event EventHandler<IndexUpdate>? IndexUpdated;

    public async Task StartListeningAsync()
    {
        _client = new ServiceBusClient(ConnectionString);
        var subscription = await GetCreateSubscriptionAsync();
        _processor = _client.CreateProcessor(TopicName, subscription, new ServiceBusProcessorOptions());
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync();
    }
    
    public async Task StopListeningAsync()
    {
        if (_processor is not null)
        {
            _processor.ProcessMessageAsync -= MessageHandler;
            _processor.ProcessErrorAsync -= ErrorHandler;
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
        }

        if (_client is not null)
            await _client.DisposeAsync();
    }

    public async Task SendIndexUpdateMessageAsync(IndexUpdate message)
    {
        var client = new ServiceBusClient(ConnectionString);
        var sender = client.CreateSender(TopicName);
        var jsonMessage = JsonSerializer.Serialize(message);
        var busMessage = new ServiceBusMessage(jsonMessage);

        try
        {
            await sender.SendMessageAsync(busMessage);
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
    
    private async Task<string> GetCreateSubscriptionAsync()
    {
        var adminClient = new ServiceBusAdministrationClient(ConnectionString);
        var topicExists = await adminClient.TopicExistsAsync(TopicName);
        
        if (!topicExists)
        {
            await adminClient.CreateTopicAsync(TopicName);
        }
        
        var subscriptionExists = await adminClient.SubscriptionExistsAsync(TopicName,
            _currentNode.NodeName);

        if (!subscriptionExists) return _currentNode.NodeName;
        
        await adminClient.CreateSubscriptionAsync(TopicName, 
            _currentNode.NodeName);
        return _currentNode.NodeName;

    }
    
    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var jsonBody = args.Message.Body.ToString();
        var indexUpdateMessage = JsonSerializer.Deserialize<IndexUpdate>(jsonBody);

        await args.CompleteMessageAsync(args.Message);
        if (indexUpdateMessage is not null)
            IndexUpdated?.Invoke(this, indexUpdateMessage);
    }
    
    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}