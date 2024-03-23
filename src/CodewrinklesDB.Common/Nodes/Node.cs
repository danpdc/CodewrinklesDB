namespace CodewrinklesDB.Common.Nodes;

public class Node : IEquatable<Node>
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
    public Guid NodeId { get; }
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

    public bool Equals(Node? other)
    {
        if (other is null) return false;
        return NodeId == other.NodeId;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Node);
    }

    public override int GetHashCode()
    {
        return NodeId.GetHashCode();
    }

    public static bool operator ==(Node? left, Node? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Node? left, Node? right)
    {
        return !Equals(left, right);
    }
}