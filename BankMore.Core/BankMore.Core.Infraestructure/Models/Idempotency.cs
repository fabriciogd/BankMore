using BankMore.Core.Infraestructure.Contracts;

namespace BankMore.Core.Infraestructure.Models;

internal sealed class Idempotency : IIdempotency
{
    public string Key { get; set; }
}
