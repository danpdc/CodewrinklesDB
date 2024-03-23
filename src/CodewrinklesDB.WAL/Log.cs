namespace CodewrinklesDB.WAL;

public class Log
{
    public Log(int key, LogType logType, string data, DateTimeOffset timeStamp)
    {
        Key = key;
        Data = data;
        TimeStamp = timeStamp;
        LogType = logType;
    }
    public int Key { get; set; }
    public string Data { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public LogType LogType { get; set; }

    public static Log CreateFromString(string logString)
    {
        var parts = logString.Split(',');
        return new Log(int.Parse(parts[0]), (LogType)Enum.Parse(typeof(LogType), parts[1]) ,parts[2], 
            DateTimeOffset.Parse(parts[3]));
    }
    
    public override string ToString()
    {
        return $"{Key},{LogType},{Data},{TimeStamp.ToString()}";
    }
}