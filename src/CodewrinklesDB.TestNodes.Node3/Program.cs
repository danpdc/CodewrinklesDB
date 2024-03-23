using System.Diagnostics;
using System.Net;
using CodewrinklesDB.NodeManagement.Extensions;
using CodewrinklesDB.NodeManagement.Infra.Extensions;
using CodewrinklesDB.NodeManagement.Nodes;
using CodewrinklesDB.Persistence.Mongo.Extensions;
using CodewrinklesDB.TestNodes.Node3;
using CodewrinklesDB.WAL.Extensions;
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
builder.Services.RegisterWriteAheadLog();
builder.Services.AddPersistenceServices(builder.Configuration);

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
    Guid nodeId = Guid.Parse("a078c882-38f6-43c3-9052-4ac306466c2f");
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

    return new Node(nodeId, ipAddress, 12345, "Node3", capacity, ClusterRole.Unregistered,
        "Test node 3");
}