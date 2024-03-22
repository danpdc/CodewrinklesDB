namespace CodewrinklesDB.NodeManagement.Abstractions;

public interface INodeRepository
{
    Task<bool> IsNodePendingAcceptanceAsync(Guid nodeId);
}