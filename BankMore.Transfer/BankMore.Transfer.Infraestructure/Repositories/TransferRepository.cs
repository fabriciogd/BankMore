using BankMore.Core.Application.Iterfaces;
using BankMore.Transfer.Domain.Repostories;
using Dapper;

namespace BankMore.Transfer.Infraestructure.Repositories;

internal sealed class TransferRepository(IDbConnectionFactory connectionFactory) : ITransferRepository
{
    public async Task<int> AddAsync(Domain.Entities.Transfer entity, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO Transfers (SouceCheckingAccountId, DestinationCheckingAccountId, Date, Value)
            VALUES (@SouceCheckingAccountId, @DestinationCheckingAccountId, @Date, @Value);
            SELECT last_insert_rowid();
        """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        var id = await connection.ExecuteScalarAsync<int>(sql, entity);

        entity.Id = id;

        return id;
    }

    public async Task<Domain.Entities.Transfer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM Transfers WHERE Id = @Id";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<Domain.Entities.Transfer>(sql, new { Id = id });
    }

    public async Task RemoveAsync(Domain.Entities.Transfer entity)
    {
        const string sql = "DELETE FROM Transfers WHERE Id = @Id";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteAsync(sql, new { entity.Id });
    }

    public async Task UpdateAsync(Domain.Entities.Transfer entity)
    {
        const string sql = """
                UPDATE Transfers
                SET SouceCheckingAccountId = @SouceCheckingAccountId
                    DestinationCheckingAccountId = @DestinationCheckingAccountId,
                    Date = @Date,
                    Value = @Value
                WHERE Id = @Id
            """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteAsync(sql, entity);
    }
}
