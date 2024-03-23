namespace CodewrinklesDB.WAL.Files;

public static class FileLogger
{
    public static async Task InsertLogAsync(Log log, string logFilePath)
    {
        await using var streamWriter = new StreamWriter(logFilePath, true);
        await streamWriter.WriteLineAsync(log.ToString());
        await streamWriter.FlushAsync();
    }

}