namespace CodewrinklesDB.Persistence.Mongo;

public class MongoDbSettings
{
    public const string Settings = "MongoDbSettings";
    
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? PendingAcceptanceCollection { get; set; }
    public string? ActiveNodesCollection { get; set; }
    
}