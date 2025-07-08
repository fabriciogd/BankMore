using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Application.Iterfaces;
using Dapper;

namespace BankMore.Account.Infraestructure.Repositories;

internal sealed class MovementRepository(IDbConnectionFactory connectionFactory) : IMovementRepository
{
    public async Task<int> AddAsync(MovementAccount entity, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO Movements (CheckingAccountId, Date, Type, Value)
            VALUES (@CheckingAccountId, @Date, @Type, @Value);
            SELECT last_insert_rowid();
        """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        var id = await connection.ExecuteScalarAsync<int>(sql, entity);

        return id;
    }

    public async Task<decimal> GetBalanceAsync(int accountId)
    {
        const string sql = """
            SELECT IFNULL(SUM(CASE WHEN Type = 'C' THEN Value ELSE -Value END), 0)
            FROM Movements
            WHERE CheckingAccountId = @Id;
        """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.ExecuteScalarAsync<decimal>(sql, new { Id = accountId });
    }

    public async Task<MovementAccount?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM Movements WHERE Id = @Id";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<MovementAccount>(sql, new { Id = id });
    }

    public async Task RemoveAsync(MovementAccount entity)
    {
        const string sql = "DELETE FROM Movements WHERE Id = @Id";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteAsync(sql, new { entity.Id });
    }

    public async Task UpdateAsync(MovementAccount entity)
    {
        const string sql = """
            UPDATE Movements
            SET CheckingAccountId = @CheckingAccountId
                Date = @Date,
                Type = @Type,
                Value = @Value
            WHERE Id = @Id
        """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteAsync(sql, entity);
    }
}
