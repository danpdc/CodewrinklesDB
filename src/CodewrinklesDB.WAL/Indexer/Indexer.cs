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

    private int CurrentIndex { get; set; }

    public async Task RebuildIndexState()
    {
        var logFilePath = FileManager.GetCurrentLogFilePath();
        var index = await LogReader.ReadLastLogIndexAsync(logFilePath);
        CurrentIndex = index;
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

    public async Task<int> GetNextIndexAsync()
    {
        var newIndex = ++CurrentIndex;
        var indexUpdate = new IndexUpdate {NewIndex = newIndex};
        var jsonData = JsonSerializer.Serialize(indexUpdate);
        //var logFilePath = FileManager.GetCurrentLogFilePath();
        //var log = new Log(newIndex, LogType.IncrementIndex, jsonData, DateTimeOffset.UtcNow);
        //await FileLogger.InsertLogAsync(log, logFilePath);
        CurrentIndex = newIndex;

        try
        {
            await _messagingService.SendIndexUpdateMessageAsync(indexUpdate);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return CurrentIndex;
    }
    
    private async void OnIndexUpdated(object? sender, IndexUpdate e)
    {
        try
        {
            var newIndex = e.NewIndex;
            if (newIndex > CurrentIndex)
            {
                var jsonData = JsonSerializer.Serialize(e);
                var logFilePath = FileManager.GetCurrentLogFilePath();
                var log = new Log(newIndex, LogType.IncrementIndex, jsonData, DateTime.UtcNow);
                await FileLogger.InsertLogAsync(log, logFilePath);
                CurrentIndex = newIndex;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to insert append log for updated index");
        }
        
    }
    
}