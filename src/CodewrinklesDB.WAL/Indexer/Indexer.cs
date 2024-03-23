using System.Text.Json;
using CodewrinklesDB.WAL.Files;

namespace CodewrinklesDB.WAL.Indexer;

public class Indexer
{
    private readonly MessagingService _messagingService;
    public Indexer(MessagingService messagingService)
    {
        _messagingService = messagingService;
    }
    public int CurrentIndex { get; set; }

    public async Task RebuildIndexState()
    {
        var logFilePath = FileManager.GetCurrentLogFilePathAsync();
        var index = await LogReader.ReadLastLogIndexAsync(logFilePath);
        CurrentIndex = ++index;
    }

    public async Task StartListeningAsync()
    {
        _messagingService.IndexUpdated += OnIndexUpdated;
        await _messagingService.StartListeningAsync();
    }

    public async Task StopListeningAsync()
    {
        await _messagingService.StopListeningAsync();
    }
    
    private async void OnIndexUpdated(object? sender, IndexUpdate e)
    {
        try
        {
            var newIndex = e.NewIndex;
            var jsonData = JsonSerializer.Serialize(e);
            var logFilePath = FileManager.GetCurrentLogFilePathAsync();
            var log = new Log(newIndex, LogType.IncrementIndex, jsonData, DateTime.UtcNow);
            await FileLogger.InsertLogAsync(log, logFilePath);
            CurrentIndex = newIndex;
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to insert append log for updated index");
        }
        
    }
    
}