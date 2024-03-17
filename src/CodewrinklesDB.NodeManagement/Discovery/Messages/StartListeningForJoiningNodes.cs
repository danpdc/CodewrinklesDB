namespace CodewrinklesDB.NodeManagement.Discovery.Messages;

public class StartListeningForJoiningNodes
{
    public required CancellationToken StoppingToken { get; set; }
}