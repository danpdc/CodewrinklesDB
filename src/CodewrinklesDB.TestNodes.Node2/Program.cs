using System.Diagnostics;
using System.Net;
using CodewrinklesDB.Common.Nodes;
using CodewrinklesDB.NodeManagement.Extensions;
using CodewrinklesDB.NodeManagement.Infra.Extensions;
using CodewrinklesDB.Persistence.Mongo.Extensions;
using CodewrinklesDB.TestNodes.Node2;
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
    Guid nodeId = Guid.Parse("d7bd5691-35c5-4158-b8c6-35c4614b284f");
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