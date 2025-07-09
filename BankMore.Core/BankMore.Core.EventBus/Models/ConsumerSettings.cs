namespace BankMore.Core.EventBus.Models;

public class ConsumerSettings
{
    public string? Name { get; set; }

    public string? Queue { get; set; }

    public string? Topic { get; set; }

    public string DeadLetterQueue => $"{Queue}-dead";

    public int? Prefetch { get; set; }
}
