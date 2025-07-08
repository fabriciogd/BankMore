using BankMore.Core.Domain.Abstracts;

namespace BankMore.Core.Domain.Entities;

public class Idempotency : Entity
{
    public string? Key { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
}
