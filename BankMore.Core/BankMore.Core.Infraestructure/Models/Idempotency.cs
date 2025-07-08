namespace BankMore.Core.Infraestructure.Models;

public class Idempotency
{
    public int StatusCode { get; set; }

    public string? ResponseBody { get; set; }
}
