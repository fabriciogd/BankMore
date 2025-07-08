using BankMore.Core.Application.Services;
using BankMore.Core.Infraestructure.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace BankMore.Core.Infraestructure.Services;

internal class RedisIdempotencyService(IConnectionMultiplexer connection) : IIdempotencyService
{
    private const string Prefix = "idempotency:";
    private readonly TimeSpan _expiration = TimeSpan.FromMinutes(10);

    public async Task SaveAsync(string key, int statusCode, string responseBody)
    {
        var redis = connection.GetDatabase();

        var record = new Idempotency
        {
            StatusCode = statusCode,
            ResponseBody = responseBody
        };

        var json = JsonSerializer.Serialize(record);

        await redis.StringSetAsync(Prefix + key, json, _expiration);
    }

    public async Task<(bool exists, int statusCode, string responseBody)> TryGetAsync(string key)
    {
        var redis = connection.GetDatabase();

        var data = await redis.StringGetAsync(Prefix + key);

        if (data.IsNullOrEmpty) return (false, 0, null);

        var parsed = JsonSerializer.Deserialize<Idempotency>(data);

        return (true, parsed.StatusCode, parsed.ResponseBody);
    }
}
