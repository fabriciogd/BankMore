using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Application.Iterfaces;
using Dapper;

namespace BankMore.Account.Infraestructure.Repositories;

internal sealed class CheckingAccountRepository(IDbConnectionFactory connectionFactory) : ICheckingAccountRepository
{
    public async Task<int> AddAsync(CheckingAccount entity, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO Accounts (NumberAccount, NationalDocument, Name, IsActive, Password, Salt)
            VALUES (@NumberAccount, @NationalDocument, @Name, @IsActive, @Password, @Salt);
            SELECT last_insert_rowid();
            """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        var id = await connection.ExecuteScalarAsync<int>(sql, entity);

        return id;
    }

    public async Task<CheckingAccount?> GetByDocumentAsync(string? document)
    {
        const string sql = "SELECT * FROM Accounts WHERE NationalDocument = @Document";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<CheckingAccount>(sql, new { Document = document });
    }

    public async Task<CheckingAccount?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM Accounts WHERE Id = @Id";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<CheckingAccount>(sql, new { Id = id });
    }

    public async Task<CheckingAccount?> GetByNumberAccountAsync(int numberAccount)
    {
        const string sql = "SELECT * FROM Accounts WHERE NumberAccount = @NumberAccount";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<CheckingAccount>(sql, new { NumberAccount = numberAccount });
    }

    public async Task RemoveAsync(CheckingAccount entity)
    {
        const string sql = "DELETE FROM Accounts WHERE Id = @Id";

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteAsync(sql, new { entity.Id });
    }

    public async Task UpdateAsync(CheckingAccount entity)
    {
        const string sql = """
            UPDATE Accounts
            SET NumberAccount = @NumberAccount
                NationalDocument = @NationalDocument,
                Name = @Name,
                IsActive = @IsActive,
                Password = @Password,
                Salt = @Salt
            WHERE Id = @Id
        """;

        using var connection = await connectionFactory.CreateOpenConnectionAsync();

        await connection.ExecuteAsync(sql, entity);
    }
}
