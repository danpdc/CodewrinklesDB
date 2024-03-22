﻿using CodewrinklesDB.NodeManagement.Discovery.Messages;
using CodewrinklesDB.NodeManagement.Nodes;
using Wolverine;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class NodeDiscoveryManager : IAsyncDisposable
{
    private readonly DiscoverySender _sender;
    private readonly DiscoveryListener _listener;
    private readonly Node _activeNode;
    private readonly IMessageBus _bus;
    public NodeDiscoveryManager(DiscoverySender sender, DiscoveryListener listener, 
        Node activeNode, IMessageBus bus)
    {
        _sender = sender;
        _listener = listener;
        _activeNode = activeNode;
        _bus = bus;
    }

    public async Task AdvertiseNodeAsync(CancellationToken token)
    {
        await _sender.SendDiscoveryMessageAsync(_activeNode, token);
        var command = new StartListeningForJoiningNodes()
        {
            StoppingToken = token
        };
        await _bus.SendAsync(command);
    }

    public async Task ListenForNodeAdvertisements(CancellationToken stoppingToken)
    {
        await _listener.StartListeningAsync(stoppingToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _listener.StopListeningAsync();
    }
}