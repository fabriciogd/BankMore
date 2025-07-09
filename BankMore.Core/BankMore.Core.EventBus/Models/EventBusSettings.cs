namespace BankMore.Core.EventBus.Models;

public class EventBusSettings
{
    public string? ConnectionString { get; set; }
    public string DefaultQueue { get; set; } = "default-queue";
    public int DefaultPrefetch { get; set; } = 1;
    public static string SessionName => "EventBus";
    public List<ConsumerSettings> Consumers { get; set; } = new();
}
