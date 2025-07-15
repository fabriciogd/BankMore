using BankMore.Core.Application.Iterfaces;
using BankMore.Tariff.Consumer.Interfaces;
using Dapper;

namespace BankMore.Tariff.Consumer.Repositories;

internal sealed class TariffRepository(IDbConnectionFactory connectionFactory) : ITariffRepository
{
    public async Task<int> AddAsync(Domain.Tariff entity, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO Tariffies (CheckingAccountId, Date, Value)
            VALUES (@CheckingAccountId, @Date, @Value);
            SELECT last_insert_rowid();
        """
        ;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        var id = await connection.ExecuteScalarAsync<int>(sql, entity);

        entity.Id = id;

        return id;
    }

    public Task<Domain.Tariff?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(Domain.Tariff entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Domain.Tariff entity)
    {
        throw new NotImplementedException();
    }
}
