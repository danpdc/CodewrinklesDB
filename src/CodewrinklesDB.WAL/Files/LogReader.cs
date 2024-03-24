namespace CodewrinklesDB.WAL.Files;

public static class LogReader
{
    public static async Task<IEnumerable<Log>> ReadLogsAsync(string logFilePath)
    {
        var logs = new List<Log>();
        using var streamReader = new StreamReader(logFilePath);
        while (await streamReader.ReadLineAsync() is { } line)
        {
            var log = Log.CreateFromString(line);
            logs.Add(log);
        }
        return logs;
    }
    
    public static async Task<int> ReadLastLogIndexAsync(string logFilePath)
    {
        using var streamReader = new StreamReader(logFilePath);
        var lastLine = string.Empty;
        while (await streamReader.ReadLineAsync() is { } line)
        {
            lastLine = line;
        }

        if (lastLine == string.Empty) return 0;
        
        var lastLog = Log.CreateFromString(lastLine);
        return lastLog.Key;
    }
}