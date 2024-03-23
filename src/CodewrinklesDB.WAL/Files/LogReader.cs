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
        Log? log = null;
        while (await streamReader.ReadLineAsync() is { } line)
        {
            var currentLog = Log.CreateFromString(line);
            if (currentLog.LogType == LogType.IncrementIndex)
            {
                log = currentLog;
            }
        }
        return log?.Key ?? 0;
    }
}