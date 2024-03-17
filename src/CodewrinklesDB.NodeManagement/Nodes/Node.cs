using System.Net;

namespace CodewrinklesDB.NodeManagement.Nodes;

public class Node
{
    public Node(Guid nodeId, string ipAddress, int port, string nodeName, 
        Capacity capacity, ClusterRole clusterRole, string? nodeDescription = null)
    {
        Metadata = new Dictionary<string, string>();
        NodeId = nodeId;
        IpAddress = ipAddress;
        Port = port;
        NodeName = nodeName;
        Capacity = capacity;
        HealthStatus = HealthStatus.Healthy;
        NodeDescription = nodeDescription;
        ClusterRole = clusterRole;
    }
    public Guid NodeId { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string NodeName { get; set; }
    public string? NodeDescription { get; set; }
    public Capacity Capacity { get; set; }
    public HealthStatus HealthStatus { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public ClusterRole ClusterRole { get; set; }
    
    public void AddCustomMetadata(string key, string value)
    {
        if (!Metadata.TryAdd(key, value)) Metadata[key] = value;
    }
}