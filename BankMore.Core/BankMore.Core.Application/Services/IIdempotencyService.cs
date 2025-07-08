namespace BankMore.Core.Application.Services;

public interface IIdempotencyService
{
    Task<(bool exists, int statusCode, string responseBody)> TryGetAsync(string key);
    Task SaveAsync(string key, int statusCode, string responseBody);
}
