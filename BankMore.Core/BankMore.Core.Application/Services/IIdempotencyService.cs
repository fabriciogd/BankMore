using BankMore.Core.Domain.Entities;

namespace BankMore.Core.Application.Services;

public interface IIdempotencyService
{
    Task<Idempotency?> TryGetAsync(string key);
    Task SaveAsync(Idempotency idempotency);
}
