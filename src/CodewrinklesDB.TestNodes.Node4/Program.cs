using System.Diagnostics;
using System.Net;
using CodewrinklesDB.NodeManagement.Extensions;
using CodewrinklesDB.NodeManagement.Infra.Extensions;
using CodewrinklesDB.NodeManagement.Nodes;
using CodewrinklesDB.TestNodes.Node4;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseWolverine();

builder.Services.AddNodeManagementInfrastructure();
builder.Services.AddNodeDiscovery();
builder.Services.AddHostedService<NodeStartup>();
var node = GetNode();
builder.Services.AddSingleton(node);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

Node GetNode()
{
    Guid nodeId = Guid.Parse("1a6798e1-d9b1-4736-be2a-94e6f5c42a22");
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

    return new Node(nodeId, ipAddress, 12345, "Node4", capacity, ClusterRole.Leader,
        "Test node 2");
}