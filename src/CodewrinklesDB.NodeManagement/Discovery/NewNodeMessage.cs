using CodewrinklesDB.NodeManagement.Nodes;

namespace CodewrinklesDB.NodeManagement.Discovery;

public class NewNodeMessage
{
    public Guid MessageId { get; set; }
    public Node? Node { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    
}