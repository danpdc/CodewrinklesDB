
namespace CodewrinklesDB.WAL.Files;

public static class FileManager
{
    private const string LogDirectory = "Logs";
    private const string LogFilePathTemplate = "wal-{0}.log";
    
    public static string GetCurrentLogFilePathAsync()
    {
        var logDirectory = GetCreateLogDirectory();

        var files = Directory.GetFiles(logDirectory);
        
        return files.Length == 0 ? CreateFile(logDirectory) : GetLatestOrNewLogFile(files, logDirectory);
    }

    private static string GetCreateLogDirectory()
    {
        var directory = Directory.GetCurrentDirectory();
        var logDirectory = Path.Combine(directory, LogDirectory);

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        return logDirectory;
    }

    private static string GetLatestOrNewLogFile(IEnumerable<string> files, string logDirectory)
    {
        var latestFilePath = string.Empty;
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var fileCreationTime = fileInfo.CreationTime.ToString("dd-MM-yyyy");
            var currentDateTime = DateTime.Now.ToString("dd-MM-yyyy");
            if (fileCreationTime == currentDateTime)
            {
                latestFilePath = file;
            }
        }

        return string.IsNullOrEmpty(latestFilePath) ? CreateFile(logDirectory) : latestFilePath;
    }

    private static string CreateFile(string logDirectory)
    {
        var newLogFileName =Path.Combine(logDirectory, 
            string.Format(LogFilePathTemplate, DateTime.Now.ToString("dd-MM-yyyy")));
        File.Create(newLogFileName);
        return newLogFileName;
    }
}