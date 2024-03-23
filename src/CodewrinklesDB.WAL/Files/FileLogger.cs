namespace CodewrinklesDB.WAL.Files;

public class FileLogger
{
    public static async Task InsertLogAsync(Log log, string logFilePath)
    {
        await using var streamWriter = new StreamWriter(logFilePath);
        await streamWriter.WriteLineAsync(log.ToString());
        await streamWriter.FlushAsync();
    }

}