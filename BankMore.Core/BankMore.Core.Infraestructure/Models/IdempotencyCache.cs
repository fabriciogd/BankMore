namespace BankMore.Core.Infraestructure.Models;

public class IdempotencyCache
{
    public int StatusCode { get; set; }

    public string? ResponseBody { get; set; }
}
