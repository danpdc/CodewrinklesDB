using CodewrinklesDB.Common.Nodes;

namespace CodewrinklesDB.NodeManagement.Abstractions;

public interface INodeRepository
{
    Task<bool> IsNodePendingAcceptanceAsync(Guid nodeId);
    Task<Node> AddNodeToPendingAcceptanceAsync(Node newNode);
    Task<Node> UpdatedNodeInPendingAcceptanceAsync(Node updatedNode);
}