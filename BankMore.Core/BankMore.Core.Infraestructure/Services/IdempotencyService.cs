using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Application.Services;
using BankMore.Core.Domain.Entities;
using Dapper;

namespace BankMore.Core.Infraestructure.Services;

internal sealed class IdempotencyService(IDbConnectionFactory connectionFactory) : IIdempotencyService
{
    public async Task SaveAsync(Idempotency idempotency)
    {
        const string sql = """
            INSERT INTO Idempotencies (Key, CreatedAt, StatusCode, ResponseBody)
            VALUES (@Key, @CreatedAt, @StatusCode, @ResponseBody);
        """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteScalarAsync<int>(sql, idempotency);
    }

    public async Task<Idempotency?> TryGetAsync(string key)
    {
        const string sql = "SELECT * FROM Idempotencies WHERE Key = @Key";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<Idempotency>(sql, new { Key = key });
    }
}
