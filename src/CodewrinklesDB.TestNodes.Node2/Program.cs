using System.Diagnostics;
using System.Net;
using CodewrinklesDB.NodeManagement.Discovery;
using CodewrinklesDB.NodeManagement.Extensions;
using CodewrinklesDB.NodeManagement.Infra.Extensions;
using CodewrinklesDB.NodeManagement.Nodes;
using CodewrinklesDB.TestNodes.Node2;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddNodeManagementInfrastructure();
builder.Services.AddNodeDiscovery();
builder.Services.AddHostedService<NodeAdvertising>();

var node = GetNode();
builder.Services.AddSingleton(node);

var host = builder.Build();
host.Run();

Node GetNode()
{
    Guid nodeId = Guid.Parse("a2cf7d01-9957-4db8-ba42-058a01195760");
    string hostName = Dns.GetHostName();
    var ipAddress = Dns.GetHostEntry(hostName).AddressList[0].ToString();
    var cores = Environment.ProcessorCount;
    var memory = Environment.WorkingSet;
    var disk = Process.GetCurrentProcess().PrivateMemorySize64;
    var capacity = new Capacity
    {
        CpuCount = cores,
        AvailableMemory = memory,
        AvailableDiskSpace = disk
    };
    
    return new Node(nodeId, ipAddress, 12345, "Node2", capacity, ClusterRole.Unregistered,
        "Test node 2");
}