using System.Diagnostics;
using System.Net;
using CodewrinklesDB.NodeManagement.Extensions;
using CodewrinklesDB.NodeManagement.Infra.Extensions;
using CodewrinklesDB.NodeManagement.Nodes;
using CodewrinklesDB.TestNodes.Node1;

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
    Guid nodeId = Guid.Parse("c6934ea5-23d1-472a-a7fe-6f6815118f7e");
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

    return new Node(nodeId, ipAddress, 12345, "Node1", capacity, ClusterRole.Leader,
        "Test node 2");
}